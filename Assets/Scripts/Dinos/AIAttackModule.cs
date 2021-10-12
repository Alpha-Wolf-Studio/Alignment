using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAttackModule : AttackComponent
{
    float t = 0;
    protected Rigidbody rb = null;
    protected CustomNavMeshAgent agent = null;
    protected DinoClass dinoClass = DinoClass.Compsognathus;
    protected float startingSpeed = 0;
    protected float startingRotationSpeed = 0;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<CustomNavMeshAgent>();
        startingSpeed = agent.Speed;
        startingRotationSpeed = agent.AngularSpeed;
        GetComponent<Character>().OnDeath += AIStopAll;
        dinoClass = GetComponent<EnemyAI>().dinoType;
    }
    protected void AIAttacked()
    {
        switch (dinoClass)
        {
            case DinoClass.Compsognathus:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.CompiAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.CompiAttack), gameObject);
                break;
            case DinoClass.Dilophosaurus:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.DiloAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.DiloAttack), gameObject);
                break;
            case DinoClass.Raptor:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.RaptorAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.RaptorAttack), gameObject);
                break;
            case DinoClass.Triceratops:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.TrikeAttack))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.TrikeAttack), gameObject);
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
    void AIStopAll(DamageOrigin origin) 
    {
        StopAllCoroutines();
    }
    abstract public void StartAttackEvent();
    abstract public void StopAttackEvent();
}
