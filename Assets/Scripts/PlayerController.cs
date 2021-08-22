using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerController : MonoBehaviour
{
    public Action OnInteract;
    public Action OnInventory;
    public Action<float, bool> onShoot;
    private Rigidbody rb = null;
    private Camera camara = null;
    Character character = null;

    [Header("Movement")]
    [SerializeField] private float verticalSensitive = 2;
    [SerializeField] private float horizontalSensitive = 2;
    [SerializeField] private int minCameraClampVertical = -50;
    [SerializeField] private int maxCameraClampVertical = 50;
    [SerializeField] private float speedMovement = .1f;

    private float movH;
    private float movV;
    private float verticalLookRotation;

    public float maxCollDownShoot;
    public float currentCollDownShoot = 10;
    private float damageForShoot = 7;
    public bool playerInput = true;
    private float maxDistInteract = 50;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        character = GetComponent<Character>();
        camara = Camera.main;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        currentCollDownShoot += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            OnInventory?.Invoke();
        }

        if (playerInput)
        {
            

            if (Input.GetMouseButtonDown(0))
            {
                if (currentCollDownShoot < maxCollDownShoot) // Si no supera el CD se daña. Siempre puede disparar.
                {
                    //Debug.Log("Dispara y se Daña");
                    onShoot?.Invoke(maxCollDownShoot, false);
                    character.TakeDamage(damageForShoot);
                }
                else
                {
                    //Debug.Log("Dispara");
                    onShoot?.Invoke(maxCollDownShoot, true);
                    currentCollDownShoot = 0;
                }
                Ray screenRay = camara.ScreenPointToRay(Input.mousePosition);
                character.Attack(screenRay.direction);
            }

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
    }

    private void FixedUpdate()
    {
        if (playerInput)
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
