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
    [SerializeField] private float speedMovement = .1f;

    private float movH;
    private float movV;
    private float verticalLookRotation;

    public float maxCoolDownShoot;
    public float currentCoolDownShoot = 10;
    private float damageForShoot = 7;
    private float maxDistInteract = 50;

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
        if (Input.GetKeyDown(KeyCode.Escape))
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInventory?.Invoke();
        }
    }
    void UpdateCoolDown()
    {
        if (currentCoolDownShoot < maxCoolDownShoot)
            currentCoolDownShoot += Time.deltaTime;
    }
    void CanAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentCoolDownShoot < maxCoolDownShoot) // Si no supera el CD se daña. Siempre puede disparar.
            {
                //Debug.Log("Dispara y se Daña");
                onShoot?.Invoke(maxCoolDownShoot, false);
                character.TakeDamage(damageForShoot);
            }
            else
            {
                //Debug.Log("Dispara");
                onShoot?.Invoke(maxCoolDownShoot, true);
                currentCoolDownShoot = 0;
            }
            Ray screenRay = camara.ScreenPointToRay(Input.mousePosition);
            character.Attack(screenRay.direction);
        }
    }
    void CanDeposite()
    {
        if (Input.GetMouseButtonDown(1))
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

            verticalLookRotation += Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * verticalSensitive, minCameraClampVertical, maxCameraClampVertical);
            camara.transform.localEulerAngles = Vector3.left * verticalLookRotation;

            if (Input.GetKey(KeyCode.W))
            {
                rb.MovePosition(transform.position + transform.forward * speedMovement);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                rb.MovePosition(transform.position - transform.forward * speedMovement);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.MovePosition(transform.position - transform.right * speedMovement);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rb.MovePosition(transform.position + transform.right * speedMovement);
            }
        }
    }
}