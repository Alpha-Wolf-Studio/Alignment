using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriAttackAI : AIAttackModule
{
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    [SerializeField] float chargePushStrenght = 600f;
    [SerializeField] float chargeSpeedMultiplier = 5f;
    [SerializeField] float ChargeStoppingDistance = 2.0f;
    [SerializeField] float chargeMinStart = 10.0f;
    [SerializeField] float chargeMinResolveTime = 2.0f;
    [SerializeField] float chargeForwardOffset = 5.0f;
    float multipliedSpeed = 0;
    float multipliedRotationSpeed = 0;
    IEnumerator ChargeCoroutine = null;
    bool canCharge = true;
    bool attackStarted = false;
    private void Start()
    {
        multipliedSpeed = agent.Speed * chargeSpeedMultiplier;
        multipliedRotationSpeed = agent.AngularSpeed;
    }
    public override void Attack(Vector3 dir, DamageInfo info)
    {
        if (canCharge)
        {
            foreach (var collider in meleeColliders)
            {
                if (collider.GetType() == typeof(ChargeAttackCollider))
                {
                    ((ChargeAttackCollider)collider).SetColliders(attackStrenght, chargePushStrenght, info.origin);
                }
            }
            if (ChargeCoroutine != null)
            {
                StopCoroutine(ChargeCoroutine);
            }
            ChargeCoroutine = Charge(dir);
            StartCoroutine(ChargeCoroutine);
            AIAttacked();
        }
    }
    IEnumerator Charge(Vector3 dir)
    {
        canCharge = false;
        anim.SetBool("Walking", false);
        anim.SetBool("Attacking", true);
        attackStarted = false;
        agent.SetDestination(transform.position);
        while (!attackStarted) 
        {
            yield return null;
        }
        agent.basicNavAgent.isStopped = false;
        agent.Speed = multipliedSpeed;
        agent.AngularSpeed = multipliedRotationSpeed;
        if (Vector3.Distance(transform.position, dir) < ChargeStoppingDistance + chargeMinStart)
        {   //En caso de tener al player demasiado cerca al empezar la carga
            float t = 0;
            do
            {
                agent.Move(transform.forward * Time.deltaTime * chargeForwardOffset);
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } while (t < chargeMinResolveTime);
        }
        do
        {
            dir.y = transform.position.y;
            agent.SetDestination(dir);
            agent.Move(transform.forward * Time.deltaTime * chargeForwardOffset); //Tengo un offset para naturalizar la direccion del enemigo
            yield return new WaitForEndOfFrame();
        } while (Vector3.Distance(transform.position, dir) > ChargeStoppingDistance);
        agent.SetDestination(transform.position);
        agent.basicNavAgent.isStopped = true;
        anim.SetBool("Melee Attack", true);
    }

    public override void StartAttackEvent()
    {
        attackStarted = true;
        AIAttacked();
        foreach (var collider in meleeColliders)
        {
            collider.StartCollider();
        }
    }
    public override void StopAttackEvent()
    {
        foreach (var collider in meleeColliders)
        {
            collider.StopCollider();
        }
        canCharge = true;
        anim.SetBool("Melee Attack", false);
    }
}
