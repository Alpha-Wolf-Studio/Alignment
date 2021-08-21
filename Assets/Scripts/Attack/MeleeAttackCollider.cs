using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCollider : MonoBehaviour
{
    float damage = 0;
    LayerMask damageMask = default;
    Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void StartCollider() 
    {
        col.enabled = true;
    }
    public void StopCollider() 
    {
        col.enabled = false;
    }
    public void SetColliders(float damage, LayerMask mask) 
    {
        this.damage = damage;
        damageMask = mask;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageComponent = other.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeDamage(damage);
        }
    }
}
