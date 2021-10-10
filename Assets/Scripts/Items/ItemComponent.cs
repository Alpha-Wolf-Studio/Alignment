﻿using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ItemComponent : MonoBehaviour
{
    private Rigidbody rb;
    private int amount;
    private int ID;
    private bool pickeable;

    private float destroyTime = 60;
    private float maxPickTime = 1;
    private float onTime;

    private IEnumerator destroyItem;
    private IEnumerator pickeableItem;
    private float maxTimeAttractor = 1;
    private float minDistaceDestroy = 1f;
    private Vector3 initialSize;

    public bool IsPickeable() => pickeable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        destroyItem = DestroyingItem();
        pickeableItem = PickeableItem();
        initialSize = transform.localScale;
    }
    private void OnEnable()
    {
        pickeable = false;
        onTime = 0;
        transform.localScale = initialSize;
        Debug.Log("Scale: " + initialSize);

        StartCoroutine(destroyItem);
        StartCoroutine(pickeableItem);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Update()
    {
        onTime += Time.deltaTime;
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
    private IEnumerator PickeableItem()
    {
        while (onTime < maxPickTime)
        {
            yield return null;
        }
        pickeable = true;
    }
    private IEnumerator DestroyingItem()
    {
        while (onTime < destroyTime)
        {
            yield return null;
        }
        DestroyItem();
    }
    public void AttractorItemToPlayer()
    {
        pickeable = false;
        onTime = 0;
        StopAllCoroutines();
        StartCoroutine(AttractorToPlayer());
    }
    private IEnumerator AttractorToPlayer()
    {
        Transform objetive = Camera.main.transform;
        float distance = Vector3.Distance(objetive.position, transform.position);
        while (onTime < maxTimeAttractor && distance > minDistaceDestroy)
        {
            //Debug.Log("Sacale: " + transform.localScale);
            Vector3 itemPos = transform.position;
            Vector3 playerPos = objetive.position;

            distance = Vector3.Distance(playerPos, itemPos);
            itemPos = Vector3.Lerp(itemPos, playerPos, onTime / maxTimeAttractor);
            transform.localScale = Vector3.Lerp(initialSize, Vector3.zero, onTime / (maxTimeAttractor / 4));
            transform.position = itemPos;

            yield return null;
        }
        DestroyItem();
    }
    void DestroyItem()
    {
        Destroy(gameObject);        // Todo: Cambiar a la Pool de objects.
    }
}