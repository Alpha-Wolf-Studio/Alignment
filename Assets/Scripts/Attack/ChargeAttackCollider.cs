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
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageComponent = collision.collider.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeArmorDamage(damage, origin);
            collision.rigidbody.AddForce(Vector3.up * pushStrenght);
        }
    }
}
