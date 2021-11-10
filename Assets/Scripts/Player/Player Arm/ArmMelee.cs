using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMelee : ArmType
{
    [SerializeField] AttackComponent meleeAttackComponent = null;
    public override void ContinuosAction(Vector3 dir, DamageInfo info)
    {
        meleeAttackComponent.Attack(dir, info);
        animator.SetTrigger("Attack");
    }

    public override void OneShootAction(Vector3 dir, DamageInfo info)
    {
        //throw new System.NotImplementedException();
    }
}
