using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] List<ArmType> allArmTypes = new List<ArmType>();
    [SerializeField] Animator anim;
    public enum ArmTypeSelection { Free = 1, Melee, Range, Size};
    ArmTypeSelection currentArmTypeSelection = ArmTypeSelection.Free;
    bool armLocked = false;
    void LockArm() => armLocked = true;
    void UnlockArm() => armLocked = false;

    private void Awake()
    {
        foreach (var armType in allArmTypes)
        {
            armType.SetAnimator(anim);
        }
    }

    public void ChangeArmType(ArmTypeSelection armType)
    {
        if (armLocked) return;
        AudioChangeArmor(false, currentArmTypeSelection);
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
    void AudioChangeArmor(bool on, ArmTypeSelection armToAudio)
    {
        switch (armToAudio)
        {
            case ArmTypeSelection.Free:
                AkSoundEngine.PostEvent(on ? AK.EVENTS.PLAYERARMEMPTYON : AK.EVENTS.PLAYERARMEMPTYOFF, gameObject);
                break;
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
        AudioChangeArmor(true, currentArmTypeSelection);
    }
}
