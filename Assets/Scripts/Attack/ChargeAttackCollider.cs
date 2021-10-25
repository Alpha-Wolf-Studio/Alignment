using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackCollider : MeleeAttackCollider
{

    float pushStrenght = 0;

    public void SetColliders(float damage, float pushStrenght, DamageOrigin origin) 
    {
        this.damage = damage;
        this.pushStrenght = pushStrenght;
        this.origin = origin;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageComponent = other.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            DamageInfo info = new DamageInfo(damage, origin, DamageType.Armor);
            damageComponent.TakeDamage(info);
            var rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForce(Vector3.up * pushStrenght);
            }
        }
    }
}
