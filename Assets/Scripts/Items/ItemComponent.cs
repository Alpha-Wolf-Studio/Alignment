using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ItemComponent : MonoBehaviour
{
    Rigidbody rb = null;
    int amount = 0;
    int ID = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
