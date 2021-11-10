using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MeleeAttackCheck : MonoBehaviour
{
    float damage = 0;
    float range = 0;
    DamageOrigin origin;
    LayerMask damageLayer;

    public void StartDamage()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, Vector3.up, range, damageLayer);
        if(hits.Length > 0) 
        {
            foreach (var hit in hits)
            {
                IDamageable damageComponent = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageComponent != null)
                {
                    damageComponent.TakeDamage(new DamageInfo(damage, origin, DamageType.Armor, transform));
                }
            }
        }
    }
    public void SetDamage(float damage, float range, DamageOrigin damageOrigin, LayerMask damageLayer)
    {
        this.damage = damage;
        this.range = range;
        this.damageLayer = damageLayer;
        origin = damageOrigin;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, range);
    }
#endif

}
