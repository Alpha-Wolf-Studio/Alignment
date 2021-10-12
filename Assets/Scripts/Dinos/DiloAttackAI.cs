using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiloAttackAI : AIAttackModule
{
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform projectileSpawn = null;
    [SerializeField] float projectileSpeed = 50f;
    public override void Attack(Vector3 dir, DamageOrigin origin)
    {
        Vector3 frontDir = dir - transform.position;
        agent.SetDestination(transform.position);
        AIAimToAttack(frontDir);
        if (canAttack)
        {
            StartCoroutine(CooldownCoroutine());
            GameObject go = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
            go.GetComponent<Projectile>().Launch(frontDir, projectileSpeed, attackStrenght, origin);
            AIAttacked();
        }
    }
    public override void StartAttackEvent()
    {

    }
    public override void StopAttackEvent()
    {

    }
}
