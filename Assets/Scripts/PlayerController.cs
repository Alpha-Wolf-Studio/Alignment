using System;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerStatus { Fading, Inization, Game, Pause, Inventory, EndWin, EndLose }
    public PlayerStatus playerStatus = PlayerStatus.Inization;
    
    public Action OnInteract;
    public Action OnInventory;
    public Action OnPause;
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
    private float energyRegenerateRun = 0.15f;

    private float maxStamina = 100;
    [SerializeField] private float currentStamina = 100;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
        camara = Camera.main;
    }
    void Start()
    {
        AvailableCursor(false);
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
    void CanJump()
    {
        if (Input.GetButtonDown("Jump"))
        {

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
                character.TakeEnergyDamage(damageForShoot);
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
                CanJump();
                break;
            case PlayerStatus.Inventory:
                UpdateCoolDown();
                CanInventory();
                CanPause();
                break;
            case PlayerStatus.Pause:
                CanPause();
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

            if (Input.GetAxis("Vertical") > 0)
            {
                rb.MovePosition(transform.position + transform.forward * speedMovement);
                if (useEnergyRun) currentStamina -= energySpendRun;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                rb.MovePosition(transform.position - transform.forward * speedMovement);
                if (useEnergyRun) currentStamina -= energySpendRun;
            }

            if (Input.GetAxis("Horizontal") < 0)
            {
                rb.MovePosition(transform.position - transform.right * speedMovement);
                if (useEnergyRun) currentStamina -= energySpendRun;
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                rb.MovePosition(transform.position + transform.right * speedMovement);
                if (useEnergyRun) currentStamina -= energySpendRun;
            }

            if (currentStamina < 0)
            {
                speedMovement = walkSpeedMovement;
                useEnergyRun = false;
            }

            if (!useEnergyRun)
            {
                if (currentStamina < maxStamina)
                {
                    currentStamina += energyRegenerateRun;
                    if (currentStamina > maxStamina)
                    {
                        currentStamina = maxStamina;
                    }
                }
            }
        }
    }
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