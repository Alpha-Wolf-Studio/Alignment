using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackComponent : AttackComponent
{
    [SerializeField] List<MeleeAttackCheck> meleeChecks = new List<MeleeAttackCheck>();
    [SerializeField] float range;
    public override void Attack(Vector3 dir, DamageInfo info)
    {
        foreach (var meleeAttacks in meleeChecks)
        {
            meleeAttacks.SetDamage(info.amount, range, info.origin, attackLayer);
        }
    }
    public override void Attack(Transform dirTransform, DamageInfo info)
    {
        foreach (var meleeAttacks in meleeChecks)
        {
            meleeAttacks.SetDamage(info.amount, range, info.origin, attackLayer);
        }
    }
    public void StartDamageEvent()
    {
        foreach (var meleeAttacks in meleeChecks)
        {
            meleeAttacks.StartDamage();
        }
    }
}
