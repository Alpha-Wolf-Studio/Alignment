using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriAttackAI : AIAttackModule
{
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    [SerializeField] float chargePushStrenght = 600f;
    [SerializeField] float chargeSpeedMultiplier = 5f;
    [SerializeField] float DirectChargeStoppingDistance = 2.0f;
    [SerializeField] float chargeMinStart = 10.0f;
    [SerializeField] float chargeMinResolveTime = 2.0f;
    [SerializeField] float chargeForwardOffset = 7.0f;
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
    public override void Attack(Transform dirTransform, DamageInfo info)
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
            ChargeCoroutine = Charge(dirTransform);
            StartCoroutine(ChargeCoroutine);
            AIAttacked();
        }
    }
    IEnumerator Charge(Transform dirTransform)
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
        agent.Speed = multipliedSpeed;
        agent.AngularSpeed = multipliedRotationSpeed;
        if (Vector3.Distance(transform.position, dirTransform.position) < chargeMinStart)
        {   //En caso de tener al player demasiado cerca al empezar la carga
            do
            {
                agent.Move(transform.forward * Time.deltaTime * chargeForwardOffset);
                agent.SetDestination(transform.position + transform.forward);
                yield return new WaitForEndOfFrame();
            } while (Vector3.Distance(transform.position, dirTransform.position) < chargeMinStart);
        }
        Vector3 dir;
        do
        {
            dir = dirTransform.position;
            dir.y = transform.position.y;
            agent.SetDestination(dir);
            agent.Move(transform.forward * Time.deltaTime * chargeForwardOffset); //Tengo un offset para naturalizar la direccion del enemigo
            yield return new WaitForEndOfFrame();
        } while (Vector3.Distance(transform.position, dir) > DirectChargeStoppingDistance);
        float p = 0;
        do
        {
            agent.Move(transform.forward * Time.deltaTime * chargeForwardOffset); //Al estar cerca del objetivo continua derecho
            agent.SetDestination(transform.position + transform.forward);
            p += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (p < chargeMinResolveTime);
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
