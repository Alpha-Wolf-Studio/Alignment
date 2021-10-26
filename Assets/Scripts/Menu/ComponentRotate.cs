using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ComponentRotate : MonoBehaviour
{
    private enum Axis
    {
        Random,
        X,
        Y,
        Z
    }
    private Rigidbody rb;
    [SerializeField] private Axis axisForce = Axis.Random;
    [SerializeField] private float speed;
    private Vector3 force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        switch (axisForce)
        {
            case Axis.Random:
                force.x = Random.Range(-speed, speed);
                force.y = Random.Range(-speed, speed);
                force.z = Random.Range(-speed, speed);
                break;
            case Axis.X:
                force = transform.right * speed;
                break;
            case Axis.Y:
                force = transform.up * speed;
                break;
            case Axis.Z:
                force = transform.forward * speed;
                break;
            default:
                Debug.Log("Axis excede el límite.", gameObject);
                break;
        }
        rb.AddTorque(force);
    }
}