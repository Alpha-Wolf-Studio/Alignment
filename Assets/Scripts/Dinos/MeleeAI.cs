using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Character))]
public class MeleeAI : MonoBehaviour
{

    [Header("Chase Behaviour")]
    [SerializeField] float chaseDistance = 50f;
    [SerializeField] float attackDistance = 1f;
    [SerializeField] float attackStoppingTolerance = .1f;
    [Header("Patrol Behaviour")]
    [SerializeField] float maxTimeBetweenPatrols = 5f;
    [SerializeField] float minTimeBetweenPatrols = 3f;
    [SerializeField] float distanceToPatrol = 30f;
    [SerializeField] float patrolStoppingTolerance = 10f;
    float currentTimeBetweenPatrols = 0f;
    Vector3 startingPosition;
    Vector3 targetPos;
    [Space(10)]
    [SerializeField] Animator anim;
    Character character;

    [SerializeField] Transform playerTransform = null;
    NavMeshAgent agent = null;

    enum EnemyBehaviour { IDLE, PATROLLING, CHASING}
    EnemyBehaviour currentBehaviour = EnemyBehaviour.IDLE;
    float idleTime = 0;

    private void Awake()
    {
        character = GetComponent<Character>();
        agent = GetComponent<NavMeshAgent>();
        character.OnDeath += StopMoving;
        agent.stoppingDistance = attackDistance - attackStoppingTolerance;
        startingPosition = transform.position;
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
            idleTime = 0;
            currentBehaviour = EnemyBehaviour.CHASING;
            agent.SetDestination(playerTransform.position);
        }
        else if (currentBehaviour == EnemyBehaviour.IDLE)
        {
            idleTime += Time.deltaTime;
            if (idleTime > currentTimeBetweenPatrols)
            {
                currentTimeBetweenPatrols = Random.Range(minTimeBetweenPatrols, maxTimeBetweenPatrols);
                anim.SetBool("Walking", true);
                currentBehaviour = EnemyBehaviour.PATROLLING;
                targetPos = startingPosition + Random.insideUnitSphere * distanceToPatrol;
                targetPos.y = Terrain.activeTerrain.SampleHeight(targetPos);
                agent.SetDestination(targetPos);
                idleTime = 0;
            }
            else 
            {
                anim.SetBool("Attacking", false);
                anim.SetBool("Walking", false);
            }
        }

        switch (currentBehaviour)
        {
            case EnemyBehaviour.IDLE:
                break;
            case EnemyBehaviour.PATROLLING:
                float remainingDistance = Vector3.Distance(targetPos, transform.position);
                if (remainingDistance < patrolStoppingTolerance) 
                {
                    currentBehaviour = EnemyBehaviour.IDLE;
                    agent.SetDestination(transform.position);
                }
                break;
            case EnemyBehaviour.CHASING:
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
