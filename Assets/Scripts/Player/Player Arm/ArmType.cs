using UnityEngine;

[System.Serializable]
public abstract class ArmType : MonoBehaviour
{
    protected Animator animator = null;
    public void SetAnimator(Animator anim) => animator = anim;
    public abstract void OnAttackEvent();
    public abstract void OneShootAction(Vector3 dir, DamageInfo info);
    public abstract void ContinuosAction(Vector3 dir, DamageInfo info);
}
