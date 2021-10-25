public class DamageInfo 
{
    public DamageInfo(float amount, DamageOrigin origin, DamageType type) 
    {
        this.amount = amount;
        this.origin = origin;
        this.type = type;
    }

    public float amount;

    public DamageOrigin origin;

    public DamageType type;
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
