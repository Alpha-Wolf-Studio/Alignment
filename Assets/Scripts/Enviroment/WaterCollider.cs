using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    [SerializeField] float killDamage = 1000f;
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.collider.GetComponent<IDamageable>(); 
        if(damageable != null) 
        {
            damageable.TakeEnergyDamage(killDamage);
        }
    }
}
