using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerStatus { Fading, Inization, Game, Pause, Inventory, Console, EndWin, EndLose }
    public PlayerStatus playerStatus = PlayerStatus.Inization;
    
    public Action OnInventory;
    public Action OnPause;
    public Action OnOpenConsole;
    public Action OnOpenQuestPanel;
    public Action<float, bool> onShoot;

    private Rigidbody rb;
    private Camera camara;
    Character character;

    [Header("Movement")]
    [SerializeField] private float verticalSensitive = 2;
    [SerializeField] private float horizontalSensitive = 2;
    [SerializeField] private int minCameraClampVertical = -50;
    [SerializeField] private int maxCameraClampVertical = 50;
    private float speedMovement = 0.1f;
    [SerializeField] private float walkSpeedMovement = 0.1f;
    [SerializeField] private float runSpeedMovement = 0.15f;

    private float movH;
    private float movV;
    private float verticalLookRotation;

    public float maxCoolDownShoot;
    public float currentCoolDownShoot = 10;
    private float damageForShoot = 7;
    private float maxDistInteract = 50;
    
    private bool useEnergyRun;
    private float energySpendRun = 0.2f;
    private float energyRegenerate = 0.05f;

    private float maxStamina = 100;
    [SerializeField] private float currentStamina = 100;
    public enum State { Walk, Run, Flying }
    [SerializeField] private float forceJump;
    [SerializeField] private float forceFly;
    private float onTimePressFly;
    private bool useEnergyFly;
    private float energySpendFly = 0.8f;
    [SerializeField] private float maxTimePressToFly;
    private bool grounded = true;
    private bool flying;
    public bool jetpack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
        camara = Camera.main;
    }
    void Start()
    {
        AvailableCursor(false);
        currentStamina = maxStamina;
    }
    void CanPause()
    {
        if (Input.GetButtonDown("Cancel")) //(Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause?.Invoke();
        }
    }
    public void AvailableCursor(bool enable)
    {
        if (enable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    void CanInventory()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            OnInventory?.Invoke();
        }
    }
    void UpdateCoolDown()
    {
        if (currentCoolDownShoot < maxCoolDownShoot)
            currentCoolDownShoot += Time.deltaTime;
    }
    void CanRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speedMovement = runSpeedMovement;
            useEnergyRun = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedMovement = walkSpeedMovement;
            useEnergyRun = false;
        }
    }
    void CanJumpAndFly()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                //Debug.Log("Jump.");
                rb.AddForce(transform.up * forceJump, ForceMode.Impulse);
                grounded = false;
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
    void CanAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentCoolDownShoot < maxCoolDownShoot) // Si no supera el CD se daña. Siempre puede disparar.
            {
                //Debug.Log("Dispara y se Daña");
                onShoot?.Invoke(maxCoolDownShoot, false);
                character.TakeEnergyDamage(damageForShoot, DamageOrigin.PLAYER);
            }
            else
            {
                //Debug.Log("Dispara");
                onShoot?.Invoke(maxCoolDownShoot, true);
                currentCoolDownShoot = 0;
            }
            Ray screenRay = camara.ScreenPointToRay(Input.mousePosition);
            character.AttackDir(screenRay.direction);
        }
    }
    void CanDeposite()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Ray screenRay = camara.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(screenRay, out hit, maxDistInteract))
            {
                IInteractuable interact = hit.transform.GetComponent<IInteractuable>();
                if (interact == null) return;
                interact.Interact();
            }
        }
    }

    void CanOpenTask()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnOpenQuestPanel?.Invoke();
        }
    }
    void Update()
    {
        switch (playerStatus)
        {
            case PlayerStatus.Fading:
                break;
            case PlayerStatus.Inization:
                playerStatus = PlayerStatus.Game;
                break;
            case PlayerStatus.Game:
                CanAttack();
                CanDeposite();

                UpdateCoolDown();
                CanInventory();
                CanPause();
                CanRun();
                CanJumpAndFly();
                CanOpenConsole();
                CanOpenTask();
                break;
            case PlayerStatus.Inventory:
                UpdateCoolDown();
                CanInventory();
                break;
            case PlayerStatus.Pause:
                CanPause();
                break;
            case PlayerStatus.Console:
                CanOpenConsole();
                break;
            case PlayerStatus.EndWin:
            case PlayerStatus.EndLose:
            default:
                break;
        }
    }
    private void FixedUpdate()
    {
        if (playerStatus == PlayerStatus.Game)
        {
            movH = Input.GetAxis("Mouse X") * horizontalSensitive;
            transform.Rotate(0, movH, 0);

            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * verticalSensitive;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, minCameraClampVertical, maxCameraClampVertical);

            camara.transform.localEulerAngles = Vector3.left * verticalLookRotation;

            if (flying && useEnergyFly)
            {
                if (jetpack)
                {
                    rb.AddForce(transform.up * forceFly, ForceMode.Impulse);
                    //currentStamina -= energySpendFly;
                }
            }

            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0 || Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                Vector3 movementVector = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speedMovement;
                rb.MovePosition(transform.position + movementVector);
                if (useEnergyRun) currentStamina -= energySpendRun;
            }

            if (currentStamina < 0)
            {
                speedMovement = walkSpeedMovement;
                useEnergyRun = false;
                flying = false;
            }
        }
        if (playerStatus == PlayerStatus.Game || playerStatus == PlayerStatus.Inventory)
            UpdateStamina();
    }

    void CanOpenConsole()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnOpenConsole?.Invoke();
        }
    }
    void UpdateStamina()
    {
        if (!useEnergyRun)
        {
            if (currentStamina < maxStamina)
            {
                if (grounded)
                {
                    currentStamina += energyRegenerate;
                    if (currentStamina > maxStamina)
                    {
                        currentStamina = maxStamina;
                    }
                }
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(Global.LayerEquals(LayerMask.GetMask("Ground"), other.gameObject.layer))
        {
            grounded = true;
        }
    }
    public float GetCurrentStamina() => currentStamina;
    public float GetMaxStamina() => maxStamina;
    public void AddSpeed(float speed)
    {
        speedMovement += speed;
    }
    public void SetSpeed(float speed)
    {
        speedMovement = speed;
    }
    public float GetSpeed()
    {
        return speedMovement;
    }
}