using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Character))]
public class EnemyAI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float maxPlayerDistanceToUpdate = 150f;
    [Header("Attack Behaviour")]
    [SerializeField] float attackDistance = 1.85f;
    [SerializeField] float yPositionTolerance = 2.5f;
    Vector3 checkPosition = Vector3.zero;
    [Header("Chase Behaviour")]
    [SerializeField] float spotAngle = 30;
    [SerializeField] float chaseDistance = 50f;
    [SerializeField] bool groupChase = false;
    [SerializeField] float groupChaseCallDistance = 10f;
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

    public DinoClass dinoType = DinoClass.Raptor;
    DamageOrigin origin;
    int spawnIndex = 0;
    public Action<DinoClass, int> OnDied;

    enum EnemyBehaviour { IDLE, PATROLLING, GROUP_CHASING, CHASING, ATTACKING}
    [SerializeField] EnemyBehaviour currentBehaviour = EnemyBehaviour.IDLE;
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
        attackModule.StopAttackEvent();
        anim.SetBool("Walking", false);
        playerTransform = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTransform) { return; }
        checkPosition = transform.position;
        checkPosition.y += yPositionTolerance;
        float distanceToPlayer = Vector3.Distance(playerTransform.position, checkPosition);
        if(distanceToPlayer < maxPlayerDistanceToUpdate) 
        {
            anim.enabled = true;
            StateUpdates(distanceToPlayer);
            StatesChanges(distanceToPlayer);
        }
    }
    private void StatesChanges(float distanceToPlayer)
    {
        if (currentBehaviour != EnemyBehaviour.ATTACKING && distanceToPlayer < attackDistance)
        {
            anim.SetBool("Walking", false);
            currentBehaviour = EnemyBehaviour.ATTACKING;
        }
        else if (currentBehaviour != EnemyBehaviour.CHASING && distanceToPlayer < chaseDistance)
        {
            float dotProduct = Vector3.Dot(playerTransform.position - transform.position, transform.forward);
            if (dotProduct > Mathf.Cos(spotAngle))
            {
                idleTime = 0;
                agent.basicNavAgent.isStopped = false;
                anim.SetBool("Walking", true);
                attackModule.AIStopAttack();
                agent.SetDestination(playerTransform.position);
                currentBehaviour = EnemyBehaviour.CHASING;
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, groupChaseCallDistance, transform.up);
                foreach (var hit in hits)
                {
                    var enemyAI = hit.collider.GetComponent<EnemyAI>();
                    if (enemyAI) 
                    {
                        enemyAI.ActivateGroupChase();
                    }
                }
            }
        }
        else if (!agent.basicNavAgent.isStopped && currentBehaviour == EnemyBehaviour.IDLE)
        {
            agent.basicNavAgent.isStopped = true;
            anim.SetBool("Walking", false);
        }
    }
    private void StateUpdates(float distanceToPlayer)
    {
        switch (currentBehaviour)
        {
            case EnemyBehaviour.IDLE:
                idleTime += Time.deltaTime;
                if (idleTime > currentTimeBetweenPatrols)
                {
                    idleTime = 0;
                    agent.basicNavAgent.isStopped = false;
                    anim.SetBool("Walking", true);
                    currentTimeBetweenPatrols = Random.Range(minTimeBetweenPatrols, maxTimeBetweenPatrols);
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
                break;
            case EnemyBehaviour.PATROLLING:
                agent.SetDestination(targetPos);
                float distance = Vector3.Distance(targetPos, transform.position);
                if (distance < patrolStoppingTolerance)
                {
                    currentBehaviour = EnemyBehaviour.IDLE;
                }
                break;
            case EnemyBehaviour.CHASING:
                agent.SetDestination(playerTransform.position);
                ResetEnemyCheck(distanceToPlayer);
                break;
            case EnemyBehaviour.GROUP_CHASING:
                agent.SetDestination(playerTransform.position);
                break;
            case EnemyBehaviour.ATTACKING:
                Vector3 attackDirection = new Vector3(playerTransform.position.x, playerTransform.position.y + yPositionTolerance, playerTransform.position.z);
                attackModule.Attack(attackDirection, origin);
                ResetEnemyCheck(distanceToPlayer);
                break;
        }
    }

    public void ActivateGroupChase() 
    {
        if (groupChase && currentBehaviour != EnemyBehaviour.CHASING && currentBehaviour != EnemyBehaviour.ATTACKING) 
        {
            agent.basicNavAgent.isStopped = false;
            anim.SetBool("Walking", true);
            currentBehaviour = EnemyBehaviour.GROUP_CHASING;
        }
    }

    void ResetEnemyCheck(float distanceToPlayer) 
    {
        if (distanceToPlayer > chaseDistance)
        {
            idleTime = 0;
            currentBehaviour = EnemyBehaviour.IDLE;
        }
    }
    public void SetSpawnIndex(int index) 
    {
        spawnIndex = index;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Global.GizmosDisk(transform.position, transform.right, chaseDistance);
        if (groupChase) 
        {
            Gizmos.color = Color.green;
            Global.GizmosDisk(transform.position, transform.right, groupChaseCallDistance);
        }
        Gizmos.color = Color.black;
        Global.GizmosDisk(transform.position, transform.right, maxPlayerDistanceToUpdate);
    }
}
