using UnityEngine;
public class ComponentRotate : MonoBehaviour
{
    enum Axis { X, Y, Z}
    private Rigidbody rb;
    [SerializeField] private Axis axisForce = Axis.X;
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
            case Axis.X:
                force = transform.right* speed;
                break;
            case Axis.Y:
                force = transform.up* speed;
                break;
            case Axis.Z:
                force = transform.forward* speed;
                break;
            default:
                Debug.Log("Axis excede el límite.", gameObject);
                break;
        }

        rb.AddTorque(force);
    }
}