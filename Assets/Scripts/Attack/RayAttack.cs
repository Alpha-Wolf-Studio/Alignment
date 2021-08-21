using UnityEngine;

public class RayAttack : AttackComponent
{

    [SerializeField] float distance = 10f;

    public override void Attack(Vector3 dir)
    {
        if(currentCooldown < 0)
        {
            StartCoroutine(CooldownCoroutine());
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, distance, attackLayer))
            {
                IDamageable damageComponent = hit.collider.GetComponent<IDamageable>();
                if (damageComponent != null)
                {
                    damageComponent.TakeDamage(attackStrenght);
                }
            }
        }
    }
}
