using UnityEngine;

public class MeleeAttackCollider : MonoBehaviour
{
    protected float damage = 0;
    protected DamageOrigin origin;
    Collider col;

    private void Awake()
    {
        col =  GetComponent<Collider>();
    }

    public void StartCollider() 
    {
        col.enabled = true;
    }
    public void StopCollider() 
    {
        col.enabled = false;
    }
    public void SetColliders(float damage, DamageOrigin damageOrigin) 
    {
        this.damage = damage;
        origin = damageOrigin;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageComponent = other.gameObject.GetComponent<IDamageable>();
        if (damageComponent != null)
        {
            damageComponent.TakeDamage(new DamageInfo(damage, origin, DamageType.Armor, transform));
        }
    }
}
