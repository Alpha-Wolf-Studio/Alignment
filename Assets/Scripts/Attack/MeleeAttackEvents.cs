using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackEvents: MonoBehaviour
{
    [SerializeField] AIAttackModule aiAttackModule = null;

    void StartAttackEvent()
    {
        aiAttackModule.StartAttackEvent();
    }
    void StopAttackEvent()
    {
        aiAttackModule.StopAttackEvent();
    }
}
