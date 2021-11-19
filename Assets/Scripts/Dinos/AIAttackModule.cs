using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAttackModule : AttackComponent
{
    float t = 0;
    [SerializeField] protected Animator anim = null;
    protected Rigidbody rb = null;
    protected CustomNavMeshAgent agent = null;
    protected DinoType dinoClass = DinoType.Compsognathus;
    protected float startingSpeed = 0;
    protected float startingRotationSpeed = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<CustomNavMeshAgent>();
        startingSpeed = agent.Speed;
        startingRotationSpeed = agent.AngularSpeed;
        GetComponent<Entity>().OnDeath += AIStopAll;
        dinoClass = GetComponent<EnemyAI>().dinoType;
    }
    protected void AIAttacked()
    {
        switch (dinoClass)
        {
            case DinoType.Compsognathus:
                AkSoundEngine.PostEvent(AK.EVENTS.COMPYATTACK, gameObject);
                break;
            case DinoType.Dilophosaurus:
                AkSoundEngine.PostEvent(AK.EVENTS.DILOATTACK, gameObject);
                break;
            case DinoType.Raptor:
                AkSoundEngine.PostEvent(AK.EVENTS.RAPTORATTACK, gameObject);
                break;
            case DinoType.Triceratops:
                AkSoundEngine.PostEvent(AK.EVENTS.TRIKEATTACK, gameObject);
                break;
            default:
                Debug.LogWarning("No está seteado dinoClass: ", gameObject);
                break;
        }
    }
    public void AIStopAttack() 
    {
        t = 0;
        agent.Speed = startingSpeed;
        agent.AngularSpeed = startingRotationSpeed;
        anim.SetBool("Attacking", false);
        anim.SetBool("Walking", true);
    }
    protected void AIAimToAttack(Vector3 dir) 
    {
        t += Time.deltaTime;
        dir.y = transform.forward.y;
        transform.forward = Vector3.Lerp(transform.forward, dir.normalized, t);
        anim.SetBool("Walking", false);
        anim.SetBool("Attacking", true);
    }
    void AIStopAll(DamageInfo info) 
    {
        StopAllCoroutines();
    }
    abstract public void StartAttackEvent();
    abstract public void StopAttackEvent();
}
