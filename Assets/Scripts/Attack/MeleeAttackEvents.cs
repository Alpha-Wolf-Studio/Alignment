using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEvents: MonoBehaviour
{
    [SerializeField] AIAttackModule aiAttackModule = null;

    void StartCollidersDamage()
    {
        aiAttackModule.StartAttackEvent();
    }
    void StopCollidersDamage()
    {
        aiAttackModule.StopAttackEvent();
    }
}
