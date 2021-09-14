﻿using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ItemComponent : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
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
        col = GetComponent<Collider>();
        col.enabled = false;
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