using System;
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
    private float speedMovement;
    private float onTimePressFly;
    private bool flying;
    private bool walking;
    private bool isRunning;

    public bool useEnergyFly;
    public bool useEnergyRun;

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

        if (flying && useEnergyFly)
        {
            if (player.IsJetpack)
            {
                rb.AddForce(transform.up * player.GetForceFly(), ForceMode.Impulse);
            }
        }

        float velX = Mathf.Abs(Input.GetAxis("Horizontal"));
        float velY = Mathf.Abs(Input.GetAxis("Vertical"));

        if (velX > 0 || velY > 0)
        {
            if (!walking)
            {
                if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerStepsOn))
                {
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerStepsOn), gameObject);
                    //Debug.Log("Empieza a Caminar.");
                }
            }

            walking = true;
        }
        else
        {
            if (walking)
            {
                if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerStepsOff))
                {
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerStepsOff), gameObject);
                    //Debug.Log("Deja a Caminar.");
                }
            }

            walking = false;
        }

        if (velY > 0 || velX > 0)
        {
            Vector3 movementVector = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speedMovement;
            rb.MovePosition(transform.position + movementVector);
            if (useEnergyRun)
            {
                playerEntity.entityStats.GetStat(StatType.Stamina).AddCurrent(-player.GetEnergySpendRun());
            }
        }

        if (playerEntity.entityStats.GetStat(StatType.Stamina).GetCurrent() < 0)
        {
            speedMovement = playerEntity.entityStats.GetStat(StatType.Walk).GetCurrent();
            useEnergyRun = false;
            flying = false;
        }
    }
    private void OnDisable()
    {
        //if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerStepsOff))
        //{
        //    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerStepsOff), gameObject);
        //}
        //
        //if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerRunOff))
        //{
        //    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerRunOff), gameObject);
        //}
    }
    private void TryRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedMovement = playerEntity.entityStats.GetStat(StatType.Walk).GetCurrent() * playerEntity.multiplyRun;
            useEnergyRun = true;
            if (!isRunning && Sfx.Get().GetEnable(Sfx.ListSfx.PlayerJump))
            {
                isRunning = true;
                AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerRunOn), gameObject);
                Debug.Log("Empieza a Correr.");
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedMovement = playerEntity.entityStats.GetStat(StatType.Walk).GetCurrent();
            useEnergyRun = false;
            if (isRunning && Sfx.Get().GetEnable(Sfx.ListSfx.PlayerJump))
            {
                isRunning = false;
                AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerRunOff), gameObject);
                Debug.Log("Termina a Correr.");
            }
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

                if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerJump))
                {
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerJump), gameObject);
                }
            }
            else
            {
                flying = true;
                useEnergyFly = true;
            }
        }
        if (Input.GetButton("Jump"))
        {
            onTimePressFly += Time.deltaTime;
            if (onTimePressFly > maxTimePressToFly && !useEnergyFly)
            {
                useEnergyFly = true;
                flying = true;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            useEnergyFly = false;
            onTimePressFly = 0;
        }
    }
}