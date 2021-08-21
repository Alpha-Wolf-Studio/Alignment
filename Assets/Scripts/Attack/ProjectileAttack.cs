using UnityEngine;

public class ProjectileAttack : AttackComponent
{

    [Header("Projectile Setup")]
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] float projectileSpeed = 50f;

    public override void Attack(Vector3 dir)
    {
        if(currentCooldown < 0)
        {
            StartCoroutine(CooldownCoroutine());
            GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            go.GetComponent<Projectile>().Launch(dir, projectileSpeed, attackStrenght);
        }
    }
}
