using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriAttackAI : AIAttackModule
{
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    [SerializeField] float chargePushStrenght = 600f;
    [SerializeField] float chargeSpeedMultiplier = 5f;
    [SerializeField] float ChargeStoppingDistance = 2.0f;
    [SerializeField] float chargeForwardOffset = 10.0f;
    float multipliedSpeed = 0;
    float multipliedRotationSpeed = 0;
    IEnumerator ChargeCoroutine = null;
    bool canCharge = true;
    private void Start()
    {
        multipliedSpeed = agent.Speed * chargeSpeedMultiplier;
        multipliedRotationSpeed = agent.AngularSpeed;
    }
    public override void Attack(Vector3 dir, DamageOrigin origin)
    {
        if (canCharge)
        {
            foreach (var collider in meleeColliders)
            {
                if (collider.GetType() == typeof(ChargeAttackCollider))
                {
                    ((ChargeAttackCollider)collider).SetColliders(attackStrenght, chargePushStrenght, origin);
                }
            }
            if (ChargeCoroutine != null)
            {
                StopCoroutine(ChargeCoroutine);
            }
            agent.basicNavAgent.isStopped = false;
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
        dir.y = transform.position.y;
        agent.Speed = multipliedSpeed;
        agent.AngularSpeed = multipliedRotationSpeed;
        if (Vector3.Distance(transform.position, dir) < ChargeStoppingDistance + chargeForwardOffset)
        {   //En caso de tener al player demasiado cerca al empezar la carga
            float t = 0;
            do
            {
                agent.Move(transform.forward * Time.deltaTime * chargeForwardOffset);
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } while (t < 1);
        }
        do
        {
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
