using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Entity))]
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
    [SerializeField] float chaseSpeedMultiplier = 1.0f;
    float baseSpeed;
    float baseChaseSpeed;
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
    Entity entity = null;
    AIAttackModule attackModule = null;

    delegate void Behaviour(float distanceToPlayer);
    Behaviour currentBehaviourUpdate;

    public Transform playerTransform { get; set; }
    CustomNavMeshAgent agent = null;

    [Space(10)]
    public DinoType dinoType = DinoType.Raptor;
    [SerializeField] DamageOrigin origin = DamageOrigin.Raptor;
    int spawnIndex = 0;
    public Action<DinoType, int> OnDied;

    float idleTime = 0;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        agent = GetComponent<CustomNavMeshAgent>();
        attackModule = GetComponent<AIAttackModule>();
        entity.OnDeath += StopMoving;
        currentTimeBetweenPatrols = minTimeBetweenPatrols;
    }

    private void Start()
    {
        if (!playerTransform)
            playerTransform = FindObjectOfType<PlayerController>().transform;
        startingPosition = transform.position;
        entity.entityStats.SetDifficult();
        agent.Speed = entity.entityStats.GetStat(StatType.Walk).GetCurrent();
        baseSpeed = agent.Speed;
        baseChaseSpeed = agent.Speed * chaseSpeedMultiplier;
        currentBehaviourUpdate = IdleUpdate;
    }

    public void SetPlayerReference(Transform trans) 
    {
        playerTransform = trans;
    }

    public void StopMoving(DamageInfo info) 
    {
        agent.SetDestination(transform.position);
        agent.basicNavAgent.isStopped = true;
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
            currentBehaviourUpdate?.Invoke(distanceToPlayer);
        }
    }
    public void SetSpawnIndex(int index) 
    {
        spawnIndex = index;
    }

    void IdleUpdate(float distanceToPlayer)
    {
        idleTime += Time.deltaTime;
        if (idleTime > currentTimeBetweenPatrols)
        {
            currentBehaviourUpdate = PatrolUpdate;
            idleTime = 0;
            agent.basicNavAgent.isStopped = false;
            anim.SetBool("Walking", true);
            currentTimeBetweenPatrols = Random.Range(minTimeBetweenPatrols, maxTimeBetweenPatrols);
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
            agent.basicNavAgent.isStopped = true;
            agent.Speed = baseSpeed;
            anim.SetBool("Walking", false);
        }
        StartChaseCheck(distanceToPlayer);
    }

    void PatrolUpdate(float distanceToPlayer)
    {
        agent.SetDestination(targetPos);
        float distance = Vector3.Distance(targetPos, transform.position);
        if (distance < patrolStoppingTolerance)
        {
            currentBehaviourUpdate = IdleUpdate;
        }
        StartChaseCheck(distanceToPlayer);
    }

    void ChaseUpdate(float distanceToPlayer) 
    {
        agent.SetDestination(playerTransform.position);
        if (distanceToPlayer < attackDistance)
        {
            anim.SetBool("Walking", false);
            currentBehaviourUpdate = AttackUpdate;
        }
        ResetChaseCheck(distanceToPlayer);
    }

    void AttackUpdate(float distanceToPlayer)
    {
        Vector3 attackDirection = new Vector3(playerTransform.position.x, playerTransform.position.y + yPositionTolerance, playerTransform.position.z);
        DamageInfo info = new DamageInfo(100, origin, DamageType.Armor, transform);
        entity.AttackDir(attackDirection, info);
        if(distanceToPlayer > attackDistance) 
        {
            StartChaseCheck(distanceToPlayer);
        }
    }

    void FrenzyUpdate(float distanceToPlayer) 
    {

    }

    public void ChangeToFrenzy() 
    {
        currentBehaviourUpdate = FrenzyUpdate;
    }

    void StartChaseCheck(float distanceToPlayer) 
    {
        if (distanceToPlayer < chaseDistance)
        {
            float dotProduct = Vector3.Dot(playerTransform.position - transform.position, transform.forward);
            if (dotProduct > Mathf.Cos(spotAngle))
            {
                idleTime = 0;
                agent.basicNavAgent.isStopped = false;
                anim.SetBool("Walking", true);
                attackModule.AIStopAttack();
                agent.SetDestination(playerTransform.position);
                agent.Speed = baseChaseSpeed;
                currentBehaviourUpdate = ChaseUpdate;
            }
        }
    }
    void ResetChaseCheck(float distanceToPlayer) 
    {
        if (distanceToPlayer > chaseDistance)
        {
            idleTime = 0;
            currentBehaviourUpdate = IdleUpdate;
            anim.SetBool("Walking", false);
            agent.SetDestination(transform.position);
            agent.basicNavAgent.isStopped = true;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, transform.up, chaseDistance);
        Handles.color = Color.black;
        Handles.DrawWireDisc(transform.position, transform.up, maxPlayerDistanceToUpdate);
    }
#endif
}
