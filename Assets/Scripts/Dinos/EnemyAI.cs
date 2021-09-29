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
    [SerializeField] float yPositionTolerance = 2.5f;
    Vector3 checkPosition = Vector3.zero;
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
    DamageOrigin origin;
    int spawnIndex = 0;
    public Action<DinoClass, int> OnDied;

    enum EnemyBehaviour { IDLE, PATROLLING, CHASING, ATTACKING}
    EnemyBehaviour currentBehaviour = EnemyBehaviour.IDLE;
    float idleTime = 0;

    private void Awake()
    {
        character = GetComponent<Character>();
        agent = GetComponent<CustomNavMeshAgent>();
        attackModule = GetComponent<AIAttackModule>();
        character.OnDeath += StopMoving;
        currentTimeBetweenPatrols = minTimeBetweenPatrols;
        switch (dinoType)
        {
            case DinoClass.Raptor:
                origin = DamageOrigin.RAPTOR;
                break;
            case DinoClass.Triceratops:
                origin = DamageOrigin.TRICERATOPS;
                break;
            case DinoClass.Dilophosaurus:
                origin = DamageOrigin.DILOPHOSAURUS;
                break;
            case DinoClass.Compsognathus:
                origin = DamageOrigin.COMPSOGNATHUS;
                break;
            default:
                break;
        }
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

    public void StopMoving(DamageOrigin origin) 
    {
        agent.SetDestination(transform.position);
        OnDied?.Invoke(dinoType, spawnIndex);
        attackModule.StopMeleeDamage();
        anim.SetBool("Walking", false);
        playerTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(!playerTransform) { return; }
        checkPosition = transform.position;
        checkPosition.y += yPositionTolerance;
        float distanceToPlayer = Vector3.Distance(playerTransform.position, checkPosition);
        if (distanceToPlayer < attackDistance)
        {
            anim.SetBool("Walking", false);
            currentBehaviour = EnemyBehaviour.ATTACKING;
        }
        else if (distanceToPlayer < chaseDistance)
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
                idleTime = 0;
                currentTimeBetweenPatrols = Random.Range(minTimeBetweenPatrols, maxTimeBetweenPatrols);
                anim.SetBool("Walking", true);
                currentBehaviour = EnemyBehaviour.PATROLLING;
                NavMeshPath path = new NavMeshPath();
                do 
                {
                    var randTarget = startingPosition + Random.insideUnitSphere * distanceToPatrol;
                    randTarget.y += 100f;
                    RaycastHit groundPosition;
                    Physics.Raycast(randTarget, Vector3.down, out groundPosition, distanceToPatrol * 100, groundLayer);
                    targetPos = groundPosition.point;
                }
                while (!agent.basicNavAgent.CalculatePath(targetPos, path) || targetPos.Equals(Vector3.zero));
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
                agent.SetDestination(targetPos);
                float distance = Vector3.Distance(targetPos, transform.position);
                if (distance < patrolStoppingTolerance) 
                {
                    currentBehaviour = EnemyBehaviour.IDLE;
                    agent.SetDestination(transform.position);
                }
                break;
            case EnemyBehaviour.CHASING:
                attackModule.StopAttack();
                break;
            case EnemyBehaviour.ATTACKING:
                attackModule.Attack(playerTransform.position, origin);
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
    }
}
