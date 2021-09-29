using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ItemComponent : MonoBehaviour
{
    private Rigidbody rb;
    int amount = 0;
    int ID = 0;
    [SerializeField] private float destroyTime = 300;
    public float maxPickTime = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void Update()
    {
        maxPickTime -= Time.deltaTime;
    }
    public void SetItem(int ID, int amount)
    {
        this.ID = ID;
        this.amount = amount;
    }
    public int GetID()
    {
        return ID;
    }
    public int GetAmount()
    {
        return amount;
    }
    public void AddForce(Vector3 force)
    {
        rb.AddForce(force);
    }
}