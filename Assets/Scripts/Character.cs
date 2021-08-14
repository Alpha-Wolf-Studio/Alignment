using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour, IDamageable
{

    public Action OnDeath;
    public Action OnTakeDamage;

    [SerializeField] Animator anim = null;
    AttackComponent attackComponent = null;

    [Header("Stats")]
    [SerializeField] float startingEnergy = 100;
    [SerializeField] float startingAttack = 5;
    [SerializeField] float startingDefense = 5;
    [SerializeField] float startingSpeed = 1;

    float maxEnergy = 100;
    float currentEnergy = 100;
    float currentAttack = 5;
    float currentDefense = 5;
    float currentSpeed = 1;

    Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        maxEnergy = startingEnergy;
        currentEnergy = startingEnergy;
        currentAttack = startingAttack;
        currentDefense = startingDefense;
        currentSpeed = startingSpeed;
        attackComponent = GetComponent<AttackComponent>();
        if (attackComponent != null)
        {
            attackComponent.SetAttackStrenght(currentAttack);
            attackComponent.SetAttackSpeed(currentSpeed);
        }
    }

    public void AddMaxEnergy(float energy)
    {
        maxEnergy += energy;
    }

    public void AddCurrentEnergy(float energy)
    {
        currentEnergy += energy;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
    }

    public void AddCurrentAttack(float attack)
    {
        currentAttack += attack;
        if (attackComponent != null) attackComponent.AddAttackStrenght(attack);
    }

    public void AddCurrentDefense(float defense)
    {
        currentDefense += defense;
    }

    public void AddCurrentSpeed(float speed)
    {
        currentSpeed += speed;
        if (attackComponent != null) attackComponent.AddAttackSpeed(speed);
    }

    public void Attack(Vector3 dir)
    {
        if (attackComponent != null) attackComponent.Attack(dir);
    }

    public void TakeDamage(float damage)
    {
        damage -= currentDefense;
        OnTakeDamage?.Invoke();
        if (damage > 0)
        {
            currentEnergy -= damage;
            if (currentEnergy > 0)
            {
                if(anim != null) anim.SetTrigger("Hit");
            }
            else
            {
                if (anim != null) anim.SetTrigger("Death");
                inventory.BlowUpInventory();
                OnDeath?.Invoke();
                Destroy(gameObject); //BORRAR - ESTO SIGUE SIENDO CODIGO DE MIERDA
            }
        }
    }
}
