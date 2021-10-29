using UnityEngine;

public class DamageInfo 
{
    public DamageInfo(float amount, DamageOrigin origin, DamageType type, Transform attackerTransform) 
    {
        this.amount = amount;
        this.origin = origin;
        this.type = type;
        this.attackerTransform = attackerTransform;
    }

    public float amount;

    public DamageOrigin origin;

    public DamageType type;

    public Transform attackerTransform;
}
public enum DamageType 
{
    Armor,
    Energy
}

public enum DamageOrigin
{
    Player,
    Raptor,
    Triceratops,
    Dilophosaurus,
    Compsognathus,
    Water
}
