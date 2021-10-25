using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackCollider : MonoBehaviour
{
    protected float damage = 0;
    protected DamageOrigin origin;
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
    public void SetColliders(float damage, DamageOrigin damageOrigin) 
    {
        this.damage = damage;
        origin = damageOrigin;
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageComponent = collision.collider.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeDamage(new DamageInfo(damage, origin, DamageType.Armor));
        }
    }
}
