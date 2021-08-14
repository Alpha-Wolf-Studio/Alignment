using System.Collections;
using UnityEngine;

public abstract class AttackComponent : MonoBehaviour
{
    protected float attackSpeed = 1;
    protected float attackStrenght = 1;
    [SerializeField] protected LayerMask attackLayer = default;
    [SerializeField] float maxCooldown = 5f;
    protected float currentCooldown = -1;

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
        currentCooldown = maxCooldown;
        while (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            yield return null;
        }
    }

    public abstract void Attack(Vector3 dir);
}
