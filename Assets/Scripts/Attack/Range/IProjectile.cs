using UnityEngine;
public interface IProjectile
{
    void Launch(Vector3 dir, float speed, float damage, DamageOrigin damageOrigin);
}
