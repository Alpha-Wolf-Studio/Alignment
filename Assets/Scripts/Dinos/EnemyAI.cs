using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(AIAttackModule))]
public class EnemyAI : MonoBehaviour
{

    [Header("Attack Behaviour")]
    [SerializeField] float attackDistance = 1.85f;
    [SerializeField] float attackStoppingTolerance = .1f;
    [SerializeField] float attackHeight = .5f;
    [Header("Chase Behaviour")]
    [SerializeField] float chaseDistance = 50f;
    [Header("Patrol Behaviour")]
    [SerializeField] float maxTimeBetweenPatrols = 5f;
    [SerializeField] float minTimeBetweenPatrols = 3f;
    [SerializeField] float distanceToPatrol = 30f;
    [SerializeField] float patrolStoppingTolerance = 10f;
    [SerializeField] LayerMask groundLayer = default;
    float currentTimeBetweenPatrols = 0f;
    Vector3 startingPosition;
    Vector3 targetPos;
    [Space(10)]
    [SerializeField] Animator anim = null;
    Character character = null;
    AIAttackModule attackModule = null;

    public Transform playerTransform { get; set; }
    CustomNavMeshAgent agent = null;

    [SerializeField] DinoClass dinoType = DinoClass.Raptor;
    int spawnIndex = 0;
    public Action<DinoClass, int> OnDied;

    enum EnemyBehaviour { IDLE, PATROLLING, CHASING}
    EnemyBehaviour currentBehaviour = EnemyBehaviour.IDLE;
    float idleTime = 0;

    private void Awake()
    {
        character = GetComponent<Character>();
        agent = GetComponent<CustomNavMeshAgent>();
        attackModule = GetComponent<AIAttackModule>();
        character.OnDeath += StopMoving;
        agent.StoppingDistance = attackDistance - attackStoppingTolerance;
        currentTimeBetweenPatrols = minTimeBetweenPatrols;
    }

    private void Start()
    {
        if (!playerTransform)
            playerTransform = FindObjectOfType<PlayerController>().transform;
        startingPosition = transform.position;
    }

    public void SetPlayerReference(Transform trans) 
    {
        playerTransform = trans;
    }

    public void StopMoving() 
    {
        agent.SetDestination(transform.position);
        OnDied?.Invoke(dinoType, spawnIndex);
        anim.SetBool("Walking", false);
        playerTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerTransform) { return; }
        float distanceSqr = Vector3.SqrMagnitude(playerTransform.position - transform.position);
        if (distanceSqr < attackDistance * attackDistance)
        {
            anim.SetBool("Walking", false);
            agent.SetDestination(transform.position);
        }
        else if (distanceSqr < chaseDistance * chaseDistance)
        {
            idleTime = 0;
            anim.SetBool("Walking", true);
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
                var randTarget = startingPosition + Random.insideUnitSphere * distanceToPatrol;
                randTarget.y += 100f;
                RaycastHit groundPosition;
                Physics.Raycast(randTarget, Vector3.down, out groundPosition, distanceToPatrol * 100, groundLayer);
                targetPos = groundPosition.point;
            }
            else 
            {
                attackModule.StopAttack();
                anim.SetBool("Walking", false);
            }
        }

        switch (currentBehaviour)
        {
            case EnemyBehaviour.IDLE:
                agent.SetDestination(transform.position);
                break;
            case EnemyBehaviour.PATROLLING:
                float remainingDistance = Vector3.Distance(targetPos, transform.position);
                agent.SetDestination(targetPos);
                if (remainingDistance < patrolStoppingTolerance) 
                {
                    currentBehaviour = EnemyBehaviour.IDLE;
                    agent.SetDestination(transform.position);
                }
                break;
            case EnemyBehaviour.CHASING:
                Vector3 startAttackPosition = new Vector3(transform.position.x, transform.position.y + attackHeight, transform.position.z);
                Vector3 endAttackPosition = new Vector3(transform.forward.x, transform.forward.y + attackHeight, transform.forward.z);
                endAttackPosition += transform.position;
                if (Physics.Raycast(startAttackPosition, endAttackPosition, attackModule.attackLayer)) 
                {
                    attackModule.Attack(playerTransform.position);
                }
                else 
                {
                    attackModule.StopAttack();
                }
                break;
            default:
                break;
        }
    }

    public void SetSpawnIndex(int index) 
    {
        spawnIndex = index;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
