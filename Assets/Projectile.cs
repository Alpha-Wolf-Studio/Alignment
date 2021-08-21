using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    float hitDamage = 0;

    public void Launch(Vector3 dir, float speed, float damage)
    {
        hitDamage = damage;
        GetComponent<Rigidbody>().AddForce(dir * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageComponent = collision.collider.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeDamage(hitDamage);
        }
    }
}
