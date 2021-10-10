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
    [SerializeField] float ChargeStoppingDistance = 2.0f;
    float startingSpeed = 0;
    float startingRotationSpeed = 0;
    float multipliedSpeed = 0;
    float multipliedRotationSpeed = 0;
    IEnumerator ChargeCoroutine = null;

    Rigidbody rb = null;
    CustomNavMeshAgent agent = null;
    private DinoClass dinoClass = DinoClass.Compsognathus;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<CustomNavMeshAgent>();
        GetComponent<Character>().OnDeath += StopAI;
        startingSpeed = agent.Speed;
        multipliedSpeed = agent.Speed * chargeSpeedMultiplier;
        startingRotationSpeed = agent.AngularSpeed;
        multipliedRotationSpeed = agent.AngularSpeed;
        dinoClass = GetComponent<EnemyAI>().dinoType;
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
                Attacked();
                break;
            case attack_Type.Charge:
                if (canAttack)
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
                    agent.basicNavAgent.isStopped = false;
                    StartCoroutine(ChargeCoroutine);

                    Attacked();
                }
                break;
            case attack_Type.Range:
                agent.SetDestination(transform.position);
                AimToAttack(frontDir);
                if (canAttack) 
                {
                    StartCoroutine(CooldownCoroutine());
                    GameObject go = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
                    go.GetComponent<Projectile>().Launch(frontDir, projectileSpeed, attackStrenght, origin);
                    Attacked();
                }
                break;
            default:
                break;
        }
    }

    void Attacked()
    {
        switch (dinoClass)
        {
            case DinoClass.Compsognathus:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.CompiAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.CompiAttack), gameObject);
                break;
            case DinoClass.Dilophosaurus:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.DiloAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.DiloAttack), gameObject);
                break;
            case DinoClass.Raptor:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.RaptorAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.RaptorAttack), gameObject);
                break;
            case DinoClass.Triceratops:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.TrikeAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.TrikeAttack), gameObject);
                break;
            default:
                Debug.LogWarning("No está seteado dinoClass: ", gameObject);
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
        anim.SetBool("Walking", false);
        anim.SetBool("Attacking", true);
        dir.y = transform.position.y;
        agent.Speed = multipliedSpeed;
        agent.AngularSpeed = multipliedRotationSpeed;
        if(Vector3.Distance(transform.position, dir) < ChargeStoppingDistance + 5f) 
        {   //En caso de tener al player demasiado cerca al empezar la carga
            float t = 0;
            do
            {
                agent.Move(transform.forward * 0.75f);
                t += Time.deltaTime;
                yield return null;
            } while (t < 2);
        }
        agent.SetDestination(dir);
        do
        {
            yield return null;
            agent.Move(transform.forward * 0.75f); //Tengo un offset para naturalizar la direccion del enemigo
        } while (Vector3.Distance(transform.position, dir) > ChargeStoppingDistance);
        agent.basicNavAgent.isStopped = true;
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
