using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    // Start is called before the first frame update

    public enum ArmTypeSelection { Free, Melee, Range};
    ArmTypeSelection currentArmTypeSelection = ArmTypeSelection.Range;
    [SerializeField] AttackComponent meleeAttackComponent;
    [SerializeField] AttackComponent rangeAttackComponent;
    bool armLocked = false;
    void LockArm() => armLocked = true;
    void UnlockArm() => armLocked = false;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeArmType(ArmTypeSelection armType)
    {
        if (!armLocked)
        {
            currentArmTypeSelection = armType;
            anim.SetInteger("Arm Type", (int)armType);
        }
    }

    public void StartArmAction(Vector3 dir, DamageInfo info) 
    {
        if (armLocked) return;
        if(currentArmTypeSelection == ArmTypeSelection.Free) 
        {
            //TODO capaz hacer el cambio en el caso de agarrar un item
        }
        else if(currentArmTypeSelection == ArmTypeSelection.Melee)
        {
            meleeAttackComponent.Attack(dir, info);
            //anim.SetTrigger("Attack");
        }
        else //if (currentArmTypeSelection == ArmTypeSelection.Range)
        {
            rangeAttackComponent.Attack(dir, info);
            //anim.SetTrigger("Attack");
            if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerAttack))
                AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerAttack), gameObject);
        }
    }



}
