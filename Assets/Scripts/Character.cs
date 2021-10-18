using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Inventory))]
public class Character : MonoBehaviour, IDamageable
{
    public Action<DamageOrigin> OnDeath;
    public Action OnUpdateStats;
    public Action OnCharacterTakeEnergyDamage;
    public Action OnCharacterTakeArmorDamage;
    public Action OnStatsLoaded;

    [Header("Animations")]
    [SerializeField] Animator anim = null;
    [SerializeField] float deadBodyRemoveTime = 5f;
    [SerializeField] float deadBodyRemoveSpeed = .25f;
    [SerializeField] float deadBodyunderGroundOffset = .5f;
    [SerializeField] AttackComponent attackComponent = null;

    [HideInInspector] public CharacterStats characterStats;
    [HideInInspector] public float multiplyRun = 2;
    
    private Inventory inventory;
    private Collider col;
    private Rigidbody rb;

    private bool isAlive = true;

    private void Awake()    // todo: Arreglar carrera de Awake con MeleeAttackCollider.cs y MeleeAttack.cs
    {
        inventory = GetComponent<Inventory>();
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        characterStats = GetComponent<CharacterStats>();
        OnStatsLoaded?.Invoke();
    }
    private void AddCurrentStat(Stat stat, float value)
    {
        stat.AddCurrent(value);
        OnUpdateStats?.Invoke();
    }
    public void AttackDir(Vector3 dir, DamageOrigin from)
    {
        if (attackComponent != null)
        {
            attackComponent.SetAttackStrenght(characterStats.GetStat(StatType.Damage).GetCurrent());
            attackComponent.SetAttackSpeed(characterStats.GetStat(StatType.AttackSpeed).GetCurrent());
            attackComponent.Attack(dir, from);
        }
    }
    public void TakeEnergyDamage(float damage, DamageOrigin damageOrigin)
    {
        if (!isAlive) return;
        OnCharacterTakeEnergyDamage?.Invoke();
        damage -= characterStats.GetStat(StatType.Defense).GetCurrent();
        if (damage > 0)
        {
            AddCurrentStat(characterStats.GetStat(StatType.Energy), -damage);
            if (characterStats.GetStat(StatType.Energy).GetCurrent() > 0)
            {
                //if(anim != null) anim.SetTrigger("Hit");
            }
            else
            {
                if (anim != null) anim.SetTrigger("Death");
                StartCoroutine(BodyRemoveCoroutine());
                inventory.BlowUpInventory();
                OnDeath?.Invoke(damageOrigin);
                isAlive = false;
            }
        }
        OnUpdateStats?.Invoke();
    }
    public void TakeArmorDamage(float damage, DamageOrigin damageOrigin)
    {
        damage -= characterStats.GetStat(StatType.Defense).GetCurrent();
        OnCharacterTakeArmorDamage?.Invoke();
        if (damage > 0 && isAlive) // todo: Agregar un daño mínimo
        {
            AddCurrentStat(characterStats.GetStat(StatType.Armor), -damage);
            if (characterStats.GetStat(StatType.Armor).GetCurrent() > 0)
            {
                //if(anim != null) anim.SetTrigger("Hit");
            }
            else
            {
                isAlive = false;
                if (anim != null) anim.SetTrigger("Death");
                StartCoroutine(BodyRemoveCoroutine());
                inventory.BlowUpInventory();
                OnDeath?.Invoke(damageOrigin);
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