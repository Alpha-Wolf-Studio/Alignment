using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttackComponent : AttackComponent
{
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    public override void Attack(Vector3 dir, DamageInfo info)
    {
        foreach (var collider in meleeColliders)
        {
            collider.SetColliders(info.amount, info.origin);
        }
    }
    public override void Attack(Transform dirTransform, DamageInfo info)
    {
        foreach (var collider in meleeColliders)
        {
            collider.SetColliders(info.amount, info.origin);
        }
    }
    public void StartAttackEvent()
    {
        foreach (var collider in meleeColliders)
        {
            collider.StartCollider();
        }
    }
    public void StopAttackEvent()
    {
        foreach (var collider in meleeColliders)
        {
            collider.StopCollider();
        }
    }
}
