using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumible", menuName = "Items/Consumible")]
public class Consumible : Item
{
    [Header("Consumible General")]
    public float currentEnergyUpgrade = 0;
    public float maxEnergyUpgrade = 0;
    public float attackUpgrade = 0;
    public float defenseUpgrade = 0;
    public float speedUpgrade = 0;
}
