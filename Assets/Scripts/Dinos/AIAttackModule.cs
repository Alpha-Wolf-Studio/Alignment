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
    [SerializeField] float chargePushStrenght = 600f;
    [SerializeField] float chargeSpeedMultiplier = 5f;
    [SerializeField] float minChargeDistance = 10f;
    [SerializeField] float chargeStoppingTolerance = 2.5f;
    float startingSpeed = 0;
    float startingRotationSpeed = 0;
    float multipliedSpeed = 0;
    float multipliedRotationSpeed = 0;
    IEnumerator ChargeCoroutine = null;

    Rigidbody rb = null;
    CustomNavMeshAgent agent = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<CustomNavMeshAgent>();
        GetComponent<Character>().OnDeath += StopAI;
        startingSpeed = agent.Speed;
        multipliedSpeed = agent.Speed * chargeSpeedMultiplier;
        startingRotationSpeed = agent.AngularSpeed;
        multipliedRotationSpeed = agent.AngularSpeed * chargeSpeedMultiplier;
    }

    public void ChangeAttackType(attack_Type newAttackType)
    {
        StopAttack();
        currentAttackType = newAttackType;
    }

    public override void Attack(Vector3 dir, DamageOrigin origin) 
    {
        Vector3 frontDir = dir - transform.position;
        switch (currentAttackType)
        {
            case attack_Type.Melee:
                agent.SetDestination(transform.position);
                AimToAttack(frontDir);
                foreach (var collider in meleeColliders)
                { 
                    collider.SetColliders(attackStrenght, origin);
                }
                break;
            case attack_Type.Charge:
                if (currentCooldown < 0)
                {
                    foreach (var collider in meleeColliders)
                    {
                        if(collider.GetType() == typeof(ChargeAttackCollider)) 
                        {
                            ((ChargeAttackCollider)collider).SetColliders(attackStrenght, chargePushStrenght, origin);
                        }
                    }
                    if(ChargeCoroutine != null) 
                    {
                        StopCoroutine(ChargeCoroutine);
                    }
                    ChargeCoroutine = Charge(dir);
                    StartCoroutine(CooldownCoroutine());
                    StartCoroutine(ChargeCoroutine);
                }
                break;
            case attack_Type.Range:
                agent.SetDestination(transform.position);
                AimToAttack(frontDir);
                if (currentCooldown < 0) 
                {
                    StartCoroutine(CooldownCoroutine());
                    GameObject go = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
                    go.GetComponent<Projectile>().Launch(frontDir, projectileSpeed, attackStrenght, origin);
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
        agent.Speed = startingSpeed;
        agent.AngularSpeed = startingRotationSpeed;
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
    IEnumerator Charge(Vector3 dir) 
    {
        dir.y = transform.position.y;
        Vector3 directionDifference = dir - transform.position;
        directionDifference += directionDifference.normalized * minChargeDistance;
        directionDifference += transform.position;

        t = 0;
        while(t < 1) 
        {
            AimToAttack(dir - transform.position);
            yield return null;
        }
        agent.Speed = multipliedSpeed;
        agent.AngularSpeed = multipliedRotationSpeed;
        agent.SetDestination(directionDifference);
        do
        {
            yield return null;
        } while (Vector3.Distance(transform.position, directionDifference) > chargeStoppingTolerance);
        anim.SetBool("Walking", false);
        anim.SetBool("Attacking", false);
    }

    void AimToAttack(Vector3 dir) 
    {
        anim.SetBool("Walking", false);
        anim.SetBool("Attacking", true);
        t += Time.deltaTime;
        dir.y = transform.forward.y;
        transform.forward = Vector3.Lerp(transform.forward, dir.normalized, t);
    }

    void StopAI(DamageOrigin origin) 
    {
        StopAllCoroutines();
    }

}
