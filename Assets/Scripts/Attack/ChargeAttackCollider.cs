using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttackCollider : MeleeAttackCollider
{

    float pushStrenght = 0;

    public void SetColliders(float damage, float pushStrenght) 
    {
        this.damage = damage;
        this.pushStrenght = pushStrenght;
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageComponent = collision.collider.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeArmorDamage(damage);
            collision.rigidbody.AddForce(Vector3.up * pushStrenght);
        }
    }
}
