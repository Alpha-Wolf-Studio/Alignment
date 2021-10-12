using System.Collections;
using UnityEngine;

public abstract class AttackComponent : MonoBehaviour
{
    public enum attack_Type { Melee, Charge, Range };

    protected float attackSpeed = 1;
    protected float attackStrenght = 1;
    public LayerMask attackLayer;
    [SerializeField] float maxCooldown = 5f;
    [SerializeField] protected attack_Type currentAttackType = attack_Type.Melee; // { get; set; }
    [SerializeField] protected Animator anim = null;
    float currentCooldown = -1;
    protected bool canAttack = true;

    public void SetAttackSpeed(float atkSpeed)
    {
        attackSpeed = atkSpeed;
    }

    public void AddAttackSpeed(float atkSpeed)
    {
        attackSpeed += atkSpeed;
    }

    public void SetAttackStrenght(float attackStr)
    {
        attackStrenght = attackStr;
    }

    public void AddAttackStrenght(float attackStr)
    {
        attackStrenght += attackStr;
    }

    protected IEnumerator CooldownCoroutine()
    {
        canAttack = false;
        currentCooldown = maxCooldown;
        while (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            yield return null;
        }
        canAttack = true;
    }

    public abstract void Attack(Vector3 dir, DamageOrigin origin);
}
