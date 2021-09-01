using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEvents: MonoBehaviour
{
    [SerializeField] AIAttackModule aiAttackModule = null;

    void StartCollidersDamage()
    {
        aiAttackModule.StartMeleeDamage();
    }
    void StopCollidersDamage()
    {
        aiAttackModule.StopMeleeDamage();
    }
}
