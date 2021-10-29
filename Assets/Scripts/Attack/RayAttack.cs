﻿using UnityEngine;

public class RayAttack : AttackComponent
{

    [SerializeField] float distance = 10f;

    public override void Attack(Vector3 dir, DamageInfo info)
    {
        if(canAttack)
        {
            StartCoroutine(CooldownCoroutine());
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, distance, attackLayer))
            {
                IDamageable damageComponent = hit.collider.GetComponent<IDamageable>();
                if (damageComponent != null)
                {
                    damageComponent.TakeDamage(new DamageInfo(attackStrenght, info.origin, DamageType.Armor, transform));
                }
            }
        }
    }
}
