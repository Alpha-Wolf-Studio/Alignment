using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    [SerializeField] float killDamage = 1000f;
    readonly DamageOrigin origin = DamageOrigin.Water;
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>(); 
        if(damageable != null) 
        {
            DamageInfo info = new DamageInfo(killDamage, origin, DamageType.Energy, transform);
            damageable.TakeDamage(info);
        }
    }
}
