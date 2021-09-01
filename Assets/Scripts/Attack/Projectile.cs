using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float hitDamage = 0;
    private float onTimeDestroy = 5.0f;

    private void Start()
    {
        Destroy(gameObject, onTimeDestroy);
    }

    public void Launch(Vector3 dir, float speed, float damage)
    {
        hitDamage = damage;
        GetComponent<Rigidbody>().AddForce(dir * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageComponent = other.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeArmorDamage(hitDamage);
        }
        Destroy(gameObject);
    }
}
