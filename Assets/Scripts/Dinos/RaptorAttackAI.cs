using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaptorAttackAI : AIAttackModule
{
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    public override void Attack(Transform dirTransform, DamageInfo info)
    {
        Vector3 frontDir = dirTransform.position - transform.position;
        agent.SetDestination(transform.position);
        AIAimToAttack(frontDir);
        foreach (var collider in meleeColliders)
        {
            collider.SetColliders(attackStrenght, info.origin);
        }
    }
    public override void StartAttackEvent()
    {
        AIAttacked();
        foreach (var collider in meleeColliders)
        {
            collider.StartCollider();
        }
    }
    public override void StopAttackEvent()
    {
        foreach (var collider in meleeColliders)
        {
            collider.StopCollider();
        }
    }
}
