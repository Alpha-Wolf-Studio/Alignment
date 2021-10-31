using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiloAttackAI : AIAttackModule
{
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform projectileSpawn = null;
    [SerializeField] float projectileSpeed = 50f;
    [SerializeField] float aimOffset = -2f;
    public override void Attack(Transform dirTransform, DamageInfo info)
    {
        Vector3 frontDir = dirTransform.position - transform.position;
        frontDir.y += aimOffset;
        agent.SetDestination(transform.position);
        AIAimToAttack(frontDir);
        if (canAttack)
        {
            StartCoroutine(CooldownCoroutine());
            GameObject go = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
            go.GetComponent<Projectile>().Launch(frontDir, projectileSpeed, attackStrenght, info.origin);
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
