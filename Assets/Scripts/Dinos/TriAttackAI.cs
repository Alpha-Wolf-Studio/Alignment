using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TriAttackAI : AIAttackModule
{
    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    [SerializeField] float chargePushStrenght = 600f;
    [SerializeField] float chargeSpeedMultiplier = 5f;
    [SerializeField] float chargeMinStart = 10.0f;
    [SerializeField] float chargeMinEnd = 1.0f;
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
            AIAttacked();
            StartCoroutine(ChargeCoroutine);
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
                agent.SetDestination(transform.position + transform.forward);
                yield return new WaitForEndOfFrame();
            } while (Vector3.Distance(transform.position, dirTransform.position) < chargeMinStart);
        }
        Vector3 dir;
        float p = 0;
        do
        {
            dir = dirTransform.position;
            dir.y = transform.position.y;
            agent.SetDestination(dir);
            p += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        } while (Vector3.Distance(transform.position, dirTransform.position) > chargeMinEnd);
        anim.SetBool("Melee Attack", true);
    }

    public override void StartAttackEvent()
    {
        attackStarted = true;
        foreach (var collider in meleeColliders)
        {
            collider.StartCollider();
        }
    }
    public override void StopAttackEvent()
    {
        canCharge = true;
        foreach (var collider in meleeColliders)
        {
            collider.StopCollider();
        }
        anim.SetBool("Melee Attack", false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, chargeMinEnd);
        Handles.color = Color.magenta;
        Handles.DrawWireDisc(transform.position, transform.up, chargeMinStart);
    }
#endif

}
