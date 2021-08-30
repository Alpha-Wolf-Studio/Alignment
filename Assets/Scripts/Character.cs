using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour, IDamageable
{
    public Action OnDeath;
    public Action OnUpdateStats;

    [Header("Animations")]
    [SerializeField] Animator anim;
    [SerializeField] float deadBodyRemoveTime = 5f;
    [SerializeField] float deadBodyRemoveSpeed = .25f;
    [SerializeField] float deadBodyunderGroundOffset = .5f;
    [SerializeField] AttackComponent attackComponent;

    [Header("Stats")]   // todo: agregar en script separado
    [SerializeField] float maxEnergy = 100;
    [SerializeField] float maxAttack = 5;
    [SerializeField] float maxDefense = 5;
    [SerializeField] float maxSpeed = 1;
    [SerializeField] float maxArmor = 5;
    
    private float currentEnergy = 100;
    private float currentAttack = 5;
    private float currentDefense = 5;
    private float currentSpeed = 1;
    private float currentArmor = 5;

    #region Getters
    public float GetMaxEnergy() => maxEnergy;
    public float GetMaxAttack() => maxAttack;
    public float GetMaxDefense() => maxDefense;
    public float GetMaxSpeed() => maxSpeed;
    public float GetMaxArmor() => maxArmor;

    public float GetEnergy() => currentEnergy;
    public float GetAttack() => currentAttack;
    public float GetDefense() => currentDefense;
    public float GetSpeed() => currentSpeed;
    public float GetArmor() => currentArmor;

    #endregion

    Inventory inventory;
    Collider col;
    Rigidbody rb;

    bool isAlive = true;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        currentEnergy = maxEnergy;
        currentArmor = maxArmor;
        currentAttack = maxAttack;
        currentDefense = maxDefense;
        currentSpeed = maxSpeed;
        if (attackComponent != null)
        {
            attackComponent.SetAttackStrenght(currentAttack);
            attackComponent.SetAttackSpeed(currentSpeed);
        }
    }
    public void AddMaxArmor(float armour)
    {
        maxArmor += armour;
        OnUpdateStats?.Invoke();
    }
    public void AddCurrentArmor(float armor)
    {
        currentArmor += armor;
        if (currentArmor > maxArmor) currentArmor = maxArmor;
        OnUpdateStats?.Invoke();
    }
    public void AddMaxEnergy(float energy)
    {
        maxEnergy += energy;
        OnUpdateStats?.Invoke();
    }
    public void AddCurrentEnergy(float energy)
    {
        currentEnergy += energy;
        if (currentEnergy > maxEnergy) currentEnergy = maxEnergy;
        OnUpdateStats?.Invoke();
    }
    public void AddCurrentAttack(float attack)
    {
        currentAttack += attack;
        if (attackComponent != null) attackComponent.AddAttackStrenght(attack);
        OnUpdateStats?.Invoke();
    }
    public void AddCurrentDefense(float defense)
    {
        currentDefense += defense;
        if (currentDefense > maxDefense) currentDefense = maxDefense;
        OnUpdateStats?.Invoke();
    }
    public void AddCurrentSpeed(float speed)
    {
        currentSpeed += speed;
        if (attackComponent != null) attackComponent.AddAttackSpeed(speed);
        OnUpdateStats?.Invoke();
    }
    public void Attack(Vector3 dir)
    {
        if (attackComponent != null) attackComponent.Attack(dir);
    }
    public void TakeDamage(float damage)
    {
        if (!isAlive) return;
        damage -= currentDefense;
        if (damage > 0)
        {
            currentEnergy -= damage;
            if (currentEnergy > 0)
            {
                //if(anim != null) anim.SetTrigger("Hit");
            }
            else
            {
                if (anim != null) anim.SetTrigger("Death");
                StartCoroutine(BodyRemoveCoroutine());
                inventory.BlowUpInventory();
                OnDeath?.Invoke();
                isAlive = false;
            }
        }
        OnUpdateStats?.Invoke();
    }
    public void TakeArmorDamage(float damage)
    {
        damage -= currentDefense;
        if (damage > 0 && isAlive) // todo: Agregar un daño mínimo
        {
            currentArmor -= damage;
            if (currentArmor > 0)
            {
                //if(anim != null) anim.SetTrigger("Hit");
            }
            else
            {
                isAlive = false;
                if (anim != null) anim.SetTrigger("Death");
                StartCoroutine(BodyRemoveCoroutine());
                inventory.BlowUpInventory();
                OnDeath?.Invoke();
            }
        }
        OnUpdateStats?.Invoke();
    }
    IEnumerator BodyRemoveCoroutine()
    {
        col.enabled = false;
        yield return new WaitForSeconds(deadBodyRemoveTime);
        rb.Sleep();
        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime * deadBodyRemoveSpeed;
            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down * deadBodyunderGroundOffset, t);
            yield return null;
        }
        Destroy(gameObject);
    }
}