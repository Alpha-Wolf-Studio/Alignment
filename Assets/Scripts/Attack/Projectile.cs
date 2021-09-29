using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float hitDamage = 0;
    DamageOrigin origin;
    private float onTimeDestroy = 5.0f;

    private void Start()
    {
        Destroy(gameObject, onTimeDestroy);
    }

    public void Launch(Vector3 dir, float speed, float damage, DamageOrigin damageOrigin)
    {
        hitDamage = damage;
        origin = damageOrigin;
        GetComponent<Rigidbody>().AddForce(dir * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageComponent = other.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeArmorDamage(hitDamage, origin);
        }
        Destroy(gameObject);
    }
}
