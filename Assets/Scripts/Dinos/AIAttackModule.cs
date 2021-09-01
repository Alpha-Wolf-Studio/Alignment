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
    [SerializeField] float groundOffset = 1f;
    [Header("Charge")]
    [SerializeField] float chargeStrenght = 1000f;

    Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ChangeAttackType(attack_Type newAttackType)
    {
        StopAttack();
        currentAttackType = newAttackType;
    }

    public override void Attack(Vector3 dir) 
    {
        t += Time.deltaTime;
        Vector3 frontDir = dir - transform.position;
        frontDir.y = dir.y + groundOffset;
        transform.forward = Vector3.Lerp(transform.forward, frontDir.normalized, t);
        switch (currentAttackType)
        {
            case attack_Type.Melee:
                foreach (var collider in meleeColliders)
                {
                    collider.SetColliders(attackStrenght);
                }
                anim.SetBool("Attacking", true);
                break;
            case attack_Type.Charge:
                if (currentCooldown < 0)
                {
                    StartCoroutine(CooldownCoroutine());
                    rb.AddForce(transform.forward * chargeStrenght);
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
        rb.velocity = Vector3.zero;
        t = 0;
        anim.SetBool("Attacking", false);
        switch (currentAttackType)
        {
            case attack_Type.Melee:
                break;
            case attack_Type.Charge:
                break;
            case attack_Type.Range:
                break;
            default:
                break;
        }
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
}
