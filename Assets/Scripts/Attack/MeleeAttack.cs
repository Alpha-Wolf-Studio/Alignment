using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : AttackComponent
{

    [SerializeField] List<MeleeAttackCollider> meleeColliders = new List<MeleeAttackCollider>();
    Collider col;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        foreach (var collider in meleeColliders)
        {
            collider.SetColliders(attackStrenght);
        }
    }

    public override void Attack(Vector3 dir)
    {
        anim.SetTrigger("Attack");
    }

    void StartCollidersDamage() 
    {
        foreach (var collider in meleeColliders)
        {
            collider.StartCollider();
        }
    }

    void StopCollidersDamage() 
    {
        foreach (var collider in meleeColliders)
        {
            collider.StopCollider();
        }
    }
}
