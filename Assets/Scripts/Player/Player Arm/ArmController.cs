using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    // Start is called before the first frame update

    public System.Action<int> OnArmChange;

    [SerializeField] List<ArmType> allArmTypes = new List<ArmType>();
    [SerializeField] Animator anim;
    public enum ArmTypeSelection { Melee = 1, Range, Size};
    ArmTypeSelection currentArmTypeSelection = ArmTypeSelection.Melee;
    bool armLocked = false;
    void LockArm() => armLocked = true;
    void UnlockArm() => armLocked = false;
    void AttackEvent() => allArmTypes[(int)currentArmTypeSelection - 1].OnAttackEvent();

    private void Awake()
    {
        foreach (var armType in allArmTypes)
        {
            armType.SetAnimator(anim);
        }
    }

    public void ChangeArmType(ArmTypeSelection armType)
    {
        if (armLocked || currentArmTypeSelection == armType) return;
        OnArmChange?.Invoke((int)armType);
        AudioChangeArm(false, currentArmTypeSelection);
        currentArmTypeSelection = armType;
        anim.SetInteger("Arm Type", (int)armType);
        Invoke(nameof(EnableAudioChangeWeapon), 0.5f);
    }

    public void StartArmOneShootAction(Vector3 dir, DamageInfo info) 
    {
        if (armLocked) return;
        allArmTypes[(int)currentArmTypeSelection - 1].OneShootAction(dir, info);
    }

    public void StartArmContinuosAction(Vector3 dir, DamageInfo info)
    {
        if (armLocked) return;
        allArmTypes[(int)currentArmTypeSelection - 1].ContinuosAction(dir, info);
    }
    void AudioChangeArm(bool on, ArmTypeSelection armToAudio)
    {
        switch (armToAudio)
        {
            //case ArmTypeSelection.Free:
                //AkSoundEngine.PostEvent(on ? AK.EVENTS.PLAYERARMEMPTYON : AK.EVENTS.PLAYERARMEMPTYOFF, gameObject);
                //break;
            case ArmTypeSelection.Melee:
                AkSoundEngine.PostEvent(on ? AK.EVENTS.PLAYERARMSWORDON : AK.EVENTS.PLAYERARMSWORDOFF, gameObject);
                break;
            case ArmTypeSelection.Range:
                AkSoundEngine.PostEvent(on ? AK.EVENTS.PLAYERARMCANNONON : AK.EVENTS.PLAYERARMCANNONOFF, gameObject);
                break;
        }
    }
    private void EnableAudioChangeWeapon()
    {
        AudioChangeArm(true, currentArmTypeSelection);
    }
}
