using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PushProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] LayerMask areaMask;
    [SerializeField] float onTimeDestroy = .5f;
    [SerializeField] float pushForce = 0.05f;
    [SerializeField] bool canHitMultipleTimes = false;
    List<IDamageable> damagedBefore = new List<IDamageable>();
    float hitDamage = 0;
    DamageOrigin origin;
    Rigidbody rb;

    private void Start()
    {
        damagedBefore.Clear();
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
        IDamageable damageComponent = other.GetComponent<IDamageable>();
        bool ignoreArea = Global.LayerEquals(areaMask, other.gameObject.layer);
        bool ignoreBullet = other.GetComponent<IProjectile>() != null;
        bool ignoreDamagedBefore = !canHitMultipleTimes && damagedBefore.Contains(damageComponent);
        if (ignoreArea || ignoreBullet || ignoreDamagedBefore) return;
        if (damageComponent != null)
        {
            damageComponent.TakeDamage(new DamageInfo(hitDamage, origin, DamageType.Armor, transform));
            if(!damagedBefore.Contains(damageComponent)) damagedBefore.Add(damageComponent);
            CustomNavMeshAgent customNavMeshAgent = other.GetComponent<CustomNavMeshAgent>();
            if (customNavMeshAgent) customNavMeshAgent.Move(rb.velocity * pushForce);
        }
    }

}