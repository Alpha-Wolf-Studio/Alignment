using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorAttackAI : AIAttackModule
{
    [SerializeField] List<MeleeAttackCheck> meleeAttackCheck = new List<MeleeAttackCheck>();
    [SerializeField] float attackRange = 1f;
    public override void Attack(Transform dirTransfom, DamageInfo info)
    {
        Vector3 frontDir = dirTransfom.position - transform.position;
        agent.SetDestination(transform.position);
        AIAimToAttack(frontDir);
        foreach (var check in meleeAttackCheck)
        {
            check.SetDamage(attackStrenght, attackRange, info.origin, attackLayer);
        }
    }

    public override void StartAttackEvent()
    {
        AIAttacked();
        foreach (var check in meleeAttackCheck)
        {
            check.StartDamage();
        }
    }

    public override void StopAttackEvent()
    {
        //throw new System.NotImplementedException();
    }
}
