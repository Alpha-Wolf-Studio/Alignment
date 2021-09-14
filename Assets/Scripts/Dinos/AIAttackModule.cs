using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackModule : AttackComponent
{
    public enum attack_Type { Melee, Charge, Range};
    [Header("General")] 
    [SerializeField] attack_Type currentAttackType = attack_Type.Melee; // { get; set; }
    [SerializeField] Animator anim = null;
    float t = 0; 
    [Header("Melee")]
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    [Header("Range")]
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform projectileSpawn = null;
    [SerializeField] float projectileSpeed = 50f;
    [Header("Charge")]
    [SerializeField] float chargeStrenght = 1f;
    [SerializeField] float chargeDistanceOffset = 1f;
    bool charging = false;

    Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Character>().OnDeath += StopAI;
    }

    public void ChangeAttackType(attack_Type newAttackType)
    {
        StopAttack();
        currentAttackType = newAttackType;
    }

    public override void Attack(Vector3 dir) 
    {
        anim.SetBool("Attacking", true);
        anim.SetBool("Walking", false);
        Vector3 frontDir = dir - transform.position;
        if (!charging) 
        {
            t += Time.deltaTime;
            frontDir.y = transform.forward.y;
            transform.forward = Vector3.Lerp(transform.forward, frontDir.normalized, t);
        }
        switch (currentAttackType)
        {
            case attack_Type.Melee:
                foreach (var collider in meleeColliders)
                {
                    collider.SetColliders(attackStrenght);
                }
                break;
            case attack_Type.Charge:
                if (currentCooldown < 0)
                {
                    StartCoroutine(CooldownCoroutine());
                    StartCoroutine(ChargeCoroutine(dir));
                }
                break;
            case attack_Type.Range:
                if(currentCooldown < 0) 
                {
                    StartCoroutine(CooldownCoroutine());
                    GameObject go = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
                    go.GetComponent<Projectile>().Launch(frontDir, projectileSpeed, attackStrenght);
                }
                break;
            default:
                break;
        }
    }

    public void StopAttack() 
    {
        t = 0;
        anim.SetBool("Attacking", false);
        anim.SetBool("Walking", true);
    }
    public void StartMeleeDamage()
    {
        foreach (var collider in meleeColliders)
        {
            collider.StartCollider();
        }
    }
    public void StopMeleeDamage()
    {
        foreach (var collider in meleeColliders)
        {
            collider.StopCollider();
        }
    }
    IEnumerator ChargeCoroutine(Vector3 dir) 
    {
        charging = true;
        rb.WakeUp();
        Vector3 aux = transform.position + dir;
        while (Vector3.Distance(transform.position, transform.position + aux) > chargeDistanceOffset) 
        {
            rb.AddForce(transform.forward * chargeStrenght);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        charging = false;
    }

    void StopAI() 
    {
        StopAllCoroutines();
    }

}
