using System;
using UnityEngine;
using System.Collections;

public class ArmRange : ArmType
{
    [SerializeField] AttackComponent rangeAttackComponent = null;
    [SerializeField] Entity playerEntity = null;
    public float maxCoolDownShoot;
    float currentCoolDownShoot;

    public static Action<float, bool> onShoot;

    private void Awake()
    {
        currentCoolDownShoot = maxCoolDownShoot;
    }

    void Update()
    {
        if (currentCoolDownShoot < maxCoolDownShoot)
            currentCoolDownShoot += Time.deltaTime;
    }
    public override void ContinuosAction(Vector3 dir, DamageInfo info)
    {
        //throw new System.NotImplementedException();
    }

    public override void OneShootAction(Vector3 dir, DamageInfo info)
    {
        rangeAttackComponent.Attack(dir, info);
        animator.SetTrigger("Attack");
        if (currentCoolDownShoot < maxCoolDownShoot)
        {
            //Debug.Log("Dispara y se Daña");
            onShoot?.Invoke(maxCoolDownShoot, false);
            playerEntity.TakeDamage(info);
        }
        else
        {
            //Debug.Log("Dispara");
            onShoot?.Invoke(maxCoolDownShoot, true);
            currentCoolDownShoot = 0;
        }
        if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerAttack))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerAttack), gameObject);
    }

}
