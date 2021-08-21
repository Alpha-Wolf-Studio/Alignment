using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCollider : MonoBehaviour
{
    float damage = 0;
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
    public void SetColliders(float damage) 
    {
        this.damage = damage;
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
