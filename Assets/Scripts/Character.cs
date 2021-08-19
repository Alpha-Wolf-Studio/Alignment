using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Character : MonoBehaviour, IDamageable
{
    public Action OnDeath;
    public Action OnTakeDamage;

    [Header("Animations")]
    [SerializeField] Animator anim = null;
    [SerializeField] float deadBodyRemoveTime = 5f;
    [SerializeField] float deadBodyRemoveSpeed = .25f;
    [SerializeField] float deadBodyunderGroundOffset = .5f;
    AttackComponent attackComponent = null;

    [Header("Stats")]
    [SerializeField] float startingEnergy = 100;
    [SerializeField] float startingAttack = 5;
    [SerializeField] float startingDefense = 5;
    [SerializeField] float startingSpeed = 1;

    private float maxEnergy = 100;
    private float currentEnergy = 100;
    private float currentAttack = 5;
    private float currentDefense = 5;
    private float currentSpeed = 1;

    #region Getters
    public float GetStartedEnergy() => startingEnergy;
    public float GetStartedAttack() => startingAttack;
    public float GetStartedDefense() => startingDefense;
    public float GetStartedSpeed() => startingSpeed;

    public float GetEnergy() => currentEnergy;
    public float GetAttack() => currentAttack;
    public float GetDefense() => currentDefense;
    public float GetSpeed() => currentSpeed;

    #endregion

    Inventory inventory;
    Rigidbody rb;
    Collider col;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
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
                //if(anim != null) anim.SetTrigger("Hit");
            }
            else
            {
                if (anim != null) anim.SetTrigger("Death");
                StartCoroutine(BodyRemoveCoroutine());
                inventory.BlowUpInventory();
                OnDeath?.Invoke();
            }
        }
    }

    IEnumerator BodyRemoveCoroutine()
    {
        yield return new WaitForSeconds(deadBodyRemoveTime);
        col.enabled = false;
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
