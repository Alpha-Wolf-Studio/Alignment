using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IProjectile
{
    [SerializeField] LayerMask areaMask;
    [SerializeField] float onTimeDestroy = 5f;
    float hitDamage = 0;
    DamageOrigin origin;
    Rigidbody rb;

    private void Start()
    {
        Destroy(gameObject, onTimeDestroy);
    }

    public void Launch(Vector3 dir, float speed, float damage, DamageOrigin damageOrigin)
    {
        hitDamage = damage;
        origin = damageOrigin;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(dir * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool ignoreCollider = Global.LayerEquals(areaMask, other.gameObject.layer) || other.GetComponent<Projectile>(); 
        if (ignoreCollider) return;
        IDamageable damageComponent = other.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeDamage(new DamageInfo(hitDamage, origin, DamageType.Armor, transform));
        }
    }
}
