using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour, IDamageable
{
    public Action OnDeath;
    public Action OnUpdateStats;
    public Action OnStatsLoaded;

    [Header("Animations")]
    [SerializeField] Animator anim = null;
    [SerializeField] float deadBodyRemoveTime = 5f;
    [SerializeField] float deadBodyRemoveSpeed = .25f;
    [SerializeField] float deadBodyunderGroundOffset = .5f;
    [SerializeField] AttackComponent attackComponent = null;

    public enum Stats { Energy, Armor, Damage, Defense, Speed }
    [Header("Stats")]
    [SerializeField] private int freePoints;

    [SerializeField] private List<Stat> stats = new List<Stat>();

    public Stat GetEnergy() => stats[(int) Stats.Energy];
    public Stat GetArmor() => stats[(int)Stats.Armor];
    public Stat GetDamage() => stats[(int)Stats.Damage];
    public Stat GetDefense() => stats[(int)Stats.Defense];
    public Stat GetSpeed() => stats[(int)Stats.Speed];

    Inventory inventory;
    Collider col;
    Rigidbody rb;

    private bool isAlive = true;

    private void Awake()    // todo: Arreglar carrera de Awake con MeleeAttackCollider.cs y MeleeAttack.cs
    {
        inventory = GetComponent<Inventory>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].Init();
        }
        if (attackComponent != null)
        {
            attackComponent.SetAttackStrenght(stats[(int)Stats.Damage].GetCurrent());
            attackComponent.SetAttackSpeed(stats[(int)Stats.Speed].GetCurrent());       // todo: speed de velocidad y de ataque es el mismo???
        }
        OnStatsLoaded?.Invoke();
    }

    private void Start()
    {

    }
    public void AddPointStats(Stats stats)
    {
        if (freePoints > 0)
        {
            freePoints--;
            this.stats[(int) stats].AddPoint();
        }
    }
    private void AddInitialStat(Stats stat, float value)
    {
        stats[(int)stat].AddInitial(value);
        OnUpdateStats?.Invoke();
    }
    private void AddCurrentStat(Stats stat, float value)
    {
        stats[(int)stat].AddCurrent(value);
        OnUpdateStats?.Invoke();
    }
    
    #region ------------------------ Add Initials ------------------------
    public void AddInitialArmor(float value) => AddInitialStat(Stats.Armor, value);
    public void AddInitialEnergy(float value) => AddInitialStat(Stats.Energy, value);
    public void AddInitialDefense(float value) => AddInitialStat(Stats.Defense, value);

    public void AddInitialAttack(float value)
    {
        AddInitialStat(Stats.Damage, value);
        if (attackComponent != null) attackComponent.AddAttackStrenght(value);
        OnUpdateStats?.Invoke();
    }

    public void AddInitialSpeed(float value)
    {
        AddInitialStat(Stats.Speed, value);
        if (attackComponent != null) attackComponent.AddAttackSpeed(value);
        OnUpdateStats?.Invoke();
    }

    #endregion

    #region ------------------------ Add Currents ------------------------
    public void AddCurrentArmor(float value) => AddCurrentStat(Stats.Armor, value);
    public void AddCurrentEnergy(float value) => AddCurrentStat(Stats.Energy, value);
    public void AddCurrentDefense(float value) => AddCurrentStat(Stats.Defense, value);
    public void AddCurrentDamage(float value) => AddCurrentStat(Stats.Damage, value);
    public void AddCurrentSpeed(float value) => AddCurrentStat(Stats.Speed, value);

    public void TopCurrentArmor() => stats[(int) Stats.Armor].SetCurrent(999);
    public void TopCurrentEnergy() => stats[(int) Stats.Energy].SetCurrent(999);
    public void TopCurrentDefense() => stats[(int) Stats.Defense].SetCurrent(999);
    public void TopCurrentDamage() => stats[(int) Stats.Damage].SetCurrent(999);
    public void TopCurrentSpeed() => stats[(int) Stats.Speed].SetCurrent(999);
    #endregion

    public void AttackDir(Vector3 dir)
    {
        if (attackComponent != null) attackComponent.Attack(dir);
    }
    public void TakeEnergyDamage(float damage)
    {
        if (!isAlive) return;
        damage -= stats[(int) Stats.Defense].GetCurrent();
        if (damage > 0)
        {
            AddCurrentStat(Stats.Energy, -damage);
            if (stats[(int)Stats.Energy].GetCurrent() > 0)
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
        damage -= stats[(int)Stats.Defense].GetCurrent();
        if (damage > 0 && isAlive) // todo: Agregar un daño mínimo
        {
            AddCurrentStat(Stats.Armor, -damage);
            if (stats[(int)Stats.Armor].GetCurrent() > 0)
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
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        yield return new WaitForSeconds(deadBodyRemoveTime);
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