using System;
using System.Collections;
using UnityEngine;
public class PlayerGame : PlayerState
{
    private PlayerController player;
    private Entity playerEntity;
    private Camera cam;
    private Rigidbody rb;

    public Action onOpenQuestPanel;
    public Action<float, bool> onShoot;

    private float maxDistInteract = 50;
    private float movH;
    private float movV;

    [SerializeField] private float maxTimePressToFly;
    [HideInInspector] public float speedMovement;
    private float onTimePressFly;

    private float velX;
    private float velZ;
    private float refTime = 0.75f;
    private float onTimeMove;
    public enum Moving
    {
        None,
        Moving,
        Flying
    }
    public Moving movement = Moving.None;
    public bool isRunning;

    [Space(10)]
    [SerializeField] private ArmController armController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        player = GetComponent<PlayerController>();
        playerEntity = GetComponent<Entity>();
    }
    private void Start()
    {
        speedMovement = playerEntity.entityStats.GetStat(StatType.Walk).GetCurrent();
    }
    private void Update()
    {
        TryOpenTask();
        TryAttack();
        TryJumpAndFly();
        TryRun();
        TryDeposite();
        player.TryPause(true);
        player.TryOnpenInventory();
    }
    private void FixedUpdate()
    {
        movH = Input.GetAxis("Mouse X") * player.GetSensitives().x;
        transform.Rotate(0, movH, 0);
        
        movV += Input.GetAxisRaw("Mouse Y") * player.GetSensitives().y;
        movV = Mathf.Clamp(movV, player.GetCameraClamp().x, player.GetCameraClamp().y);
        
        cam.transform.localEulerAngles = Vector3.left * movV;
        
        if (movement == Moving.Flying)
        {
            if (player.IsJetpack)
            {
                rb.AddForce(transform.up * player.GetForceFly(), ForceMode.Impulse);
            }
        }
        
        velX = Input.GetAxis("Horizontal");
        velZ = Input.GetAxis("Vertical");
        switch (movement)
        {
            case Moving.None:
                //Debug.Log("ESTADO: IDLE.");
                if (IsMoving())
                {
                    movement = Moving.Moving;
                    //Debug.Log("ESTADO: Empieza a Caminar.");
                    //AkSoundEngine.PostEvent(AK.EVENTS.PLAYERSTEP, gameObject);
                }
                break;
            case Moving.Moving:
                if (IsMoving())
                {
                    onTimeMove += Time.deltaTime;
                    Vector3 movementVectorWalk = (transform.forward * velZ + transform.right * velX) * speedMovement;
                    rb.MovePosition(transform.position + movementVectorWalk);
                    if (isRunning)
                    {
                        playerEntity.entityStats.GetStat(StatType.Stamina).AddCurrent(-player.GetEnergySpendRun() * Time.fixedDeltaTime);
                        //Debug.Log("ESTADO: Corriendo.");
                    }
                    else
                    {
                        //Debug.Log("ESTADO: Caminando.");
                    }

                    if (playerEntity.entityStats.GetStat(StatType.Stamina).GetCurrent() < Mathf.Epsilon)
                    {
                        speedMovement = playerEntity.entityStats.GetStat(StatType.Walk).GetCurrent();
                        isRunning = false;
                        //Debug.Log("ESTADO: Deja de Correr (Por Stamina).");
                        //Debug.Log("ESTADO: Empieza a Caminar.");
                    }
                    if (onTimeMove > refTime - speedMovement && player.IsGrounded)
                    {
                        onTimeMove = 0;
                        AkSoundEngine.PostEvent(AK.EVENTS.PLAYERSTEP, gameObject);
                    }
                }
                else
                {
                    movement = Moving.None;
                }
                break;
            case Moving.Flying:
                //Debug.Log("ESTADO: Volando.");
                Vector3 movementVectorFly = (transform.forward * velZ + transform.right * velX) * speedMovement;
                rb.MovePosition(transform.position + movementVectorFly);
                break;
        }
    }
    bool IsMoving()
    {
        bool boolReturn = (Mathf.Abs(velX) > Mathf.Epsilon || Mathf.Abs(velZ) > Mathf.Epsilon);
        //if(!boolReturn)
            //Debug.Log("Quieto.");
        return boolReturn;
    }
    private void TryRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedMovement = playerEntity.entityStats.GetStat(StatType.Walk).GetCurrent() * playerEntity.multiplyRun;
            isRunning = true;
            //Debug.Log("ESTADO: Empieza a Correr.");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedMovement = playerEntity.entityStats.GetStat(StatType.Walk).GetCurrent();
            isRunning = false;
            //Debug.Log("ESTADO: Deja de Correr.");
        }
    }
    private void TryDeposite()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(screenRay, out hit, maxDistInteract))
            {
                IInteractuable interact = hit.transform.GetComponent<IInteractuable>();
                if (interact == null) return;
                interact.Interact();
            }
        }
    }
    private void TryAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            DamageInfo info = new DamageInfo(playerEntity.entityStats.GetStat(StatType.Damage).GetCurrent(), DamageOrigin.Player, DamageType.Energy, transform);
            Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);
            playerEntity.RefreshAttackStats(ref info);
            armController.StartArmOneShootAction(screenRay.direction, info);
        }
        else if (Input.GetButton("Fire1")) 
        {
            DamageInfo info = new DamageInfo(playerEntity.entityStats.GetStat(StatType.Damage).GetCurrent(), DamageOrigin.Player, DamageType.Energy, transform);
            Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);
            playerEntity.RefreshAttackStats(ref info);
            armController.StartArmContinuosAction(screenRay.direction, info);
        }
        for (int i = 1; i < (int)ArmController.ArmTypeSelection.Size; i++)
        {
            if (Input.GetButtonDown("Arm" + i))
            {
                armController.ChangeArmType((ArmController.ArmTypeSelection)i);
            }
        }
    }
    private void TryOpenTask()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            onOpenQuestPanel?.Invoke();
        }
    }
    private void TryJumpAndFly()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (player.IsGrounded)
            {
                rb.AddForce(transform.up * player.GetForceJump(), ForceMode.Impulse);
                player.IsGrounded = false;

                AkSoundEngine.PostEvent(AK.EVENTS.PLAYERJUMP, gameObject);
            }
            else
            {
                if (player.IsJetpack)
                {
                    movement = Moving.Flying;
                    AkSoundEngine.PostEvent(AK.EVENTS.PLAYERJETPACKON, gameObject);
                    Debug.Log("Inicia a Volar 01");
                }
            }
        }

        if (Input.GetButton("Jump"))
        {
            onTimePressFly += Time.deltaTime;
            if (onTimePressFly > maxTimePressToFly)
            {
                if (player.IsJetpack)
                {
                    if (movement != Moving.Flying)
                    {
                        AkSoundEngine.PostEvent(AK.EVENTS.PLAYERJETPACKON, gameObject);
                        Debug.Log("Inicia a Volar 02");
                    }
                    movement = Moving.Flying;
                }
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            AkSoundEngine.PostEvent(AK.EVENTS.PLAYERJETPACKOFF, gameObject);
            movement = Moving.None;
            onTimePressFly = 0;
        }
    }
}