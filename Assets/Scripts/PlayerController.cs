using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera camara;

    [SerializeField] private float verticalSensitive = 2;
    [SerializeField] private float horizontalSensitive = 2;
    [SerializeField] private int minCameraClampVertical = -50;
    [SerializeField] private int maxCameraClampVertical = 50;
    [SerializeField] private float speedMovement;

    private float movH;
    private float movV;
    private float verticalLookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camara = Camera.main;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        movH = Input.GetAxis("Mouse X") * horizontalSensitive;
        transform.Rotate(0, movH, 0);

        verticalLookRotation += Mathf.Clamp(Input.GetAxisRaw("Mouse Y") * verticalSensitive, minCameraClampVertical, maxCameraClampVertical);
        camara.transform.localEulerAngles = Vector3.left * verticalLookRotation;

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * speedMovement);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(transform.forward * -speedMovement);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(transform.right * -speedMovement);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * speedMovement);
        }
    }
}
