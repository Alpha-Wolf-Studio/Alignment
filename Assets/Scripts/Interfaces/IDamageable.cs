public interface IDamageable
{
    void TakeEnergyDamage(float damage, DamageOrigin origin);
    void TakeArmorDamage(float damage, DamageOrigin origin);
}