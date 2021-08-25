using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : MonoBehaviour
{

    [Header("AI Behaviour")]
    [SerializeField] float chaseDistance = 50f;
    [SerializeField] float attackDistance = 1f;
    [SerializeField] float stoppingTolerance = .1f;
    [SerializeField] Animator anim;
    Character character;

    [SerializeField] Transform playerTransform = null;
    NavMeshAgent agent = null;

    enum EnemyBehaviour { IDLE, PATROLLING, CHASING}
    EnemyBehaviour currentBehaviour = EnemyBehaviour.IDLE;

    private void Awake()
    {
        character = GetComponent<Character>();
        agent = GetComponent<NavMeshAgent>();
        character.OnDeath += StopMoving;
        agent.stoppingDistance = attackDistance - stoppingTolerance;
    }

    public void SetPlayerReference(Transform trans) 
    {
        playerTransform = trans;
    }

    public void StopMoving() 
    {
        agent.SetDestination(transform.position);
        anim.SetBool("Walking", false);
        playerTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerTransform) { return; }
        float distanceSqr = Vector3.SqrMagnitude(playerTransform.position - transform.position);
        if (distanceSqr < chaseDistance * chaseDistance)
        {
            currentBehaviour = EnemyBehaviour.CHASING;
        }
        else 
        {
            currentBehaviour = EnemyBehaviour.IDLE;
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", false);
        }

        switch (currentBehaviour)
        {
            case EnemyBehaviour.IDLE:
                break;
            case EnemyBehaviour.PATROLLING:
                break;
            case EnemyBehaviour.CHASING:
                agent.SetDestination(playerTransform.position);
                if (distanceSqr < attackDistance * attackDistance) 
                {
                    anim.SetBool("Walking", false);
                    anim.SetBool("Attacking", true);
                }
                else 
                {
                    anim.SetBool("Walking", true);
                    anim.SetBool("Attacking", false);
                }
                break;
            default:
                break;
        }
    }

   private void OnDrawGizmos()
   {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
