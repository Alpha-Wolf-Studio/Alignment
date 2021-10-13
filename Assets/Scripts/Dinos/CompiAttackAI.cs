﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompiAttackAI : AIAttackModule
{
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    public override void Attack(Vector3 dir, DamageOrigin origin)
    {
        Vector3 frontDir = dir - transform.position;
        agent.SetDestination(transform.position);
        AIAimToAttack(frontDir);
        foreach (var collider in meleeColliders)
        {
            collider.SetColliders(attackStrenght, origin);
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