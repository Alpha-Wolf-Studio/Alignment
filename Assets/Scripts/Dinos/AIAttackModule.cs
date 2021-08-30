using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackModule : MonoBehaviour
{
    public enum attack_Type { Melee, Charge, Range};

    [SerializeField] attack_Type currentAttackType; // { get; set; }

    Animator anim;

    public void ChangeAttackType(attack_Type newAttackType)
    {
        StopAttack();
        currentAttackType = newAttackType;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void Attack() 
    {
        switch (currentAttackType)
        {
            case attack_Type.Melee:
                anim.SetBool("Attacking", true);
                break;
            case attack_Type.Charge:
                break;
            case attack_Type.Range:
                break;
            default:
                break;
        }
    }

    public void StopAttack() 
    {
        switch (currentAttackType)
        {
            case attack_Type.Melee:
                anim.SetBool("Attacking", false);
                break;
            case attack_Type.Charge:
                break;
            case attack_Type.Range:
                break;
            default:
                break;
        }
    }

}
