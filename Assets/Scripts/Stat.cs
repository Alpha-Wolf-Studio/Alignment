using UnityEngine;
[System.Serializable]
public class Stat
{
    [Header("Stat:")]
    [SerializeField] private StatType statType;
    [SerializeField] private float baseInitial;        // Base con el que empieza
    [SerializeField] private float amountPlus;         // Puntos que te da la mejora
    [SerializeField] private float multiplySubtract;   // Multiplicador que disminuye el amount

    [Header("Currents:")]
    [SerializeField] private float current;            // La cantidad actual que tiene
    [SerializeField] private float maxCurrent;         // maximo que se calcula con: CalculateMax();
    public bool modifyValues { get; set; } = true;

    public void InitStat()
    {
        current = baseInitial;
        maxCurrent = baseInitial;
    }
    public void SetToMax()
    {
        current = maxCurrent;
    }
    public void Use()
    {
        AddMax(amountPlus);
        amountPlus *= multiplySubtract;
    }
    // ----------------------------------------------------------------
    public void AddMax(float increaseIn)
    {
        float percentage = current / maxCurrent;
        maxCurrent += increaseIn;
        current = maxCurrent * percentage;
    }
    public void AddCurrent(float increaseIn)
    {
        if (modifyValues)
        {
            current += increaseIn;
            if (current > maxCurrent)
            {
                current = maxCurrent;
            }
            else if (current < 0)
            {
                current = 0;
            }
        }
    }
    // -----------------------------------------------------------------------
    public StatType GetStatsType() => statType;
    public float GetInitial() => baseInitial;
    public float GetIncrementForPoints() => amountPlus;
    public float GetMultiplySubtract() => multiplySubtract;
    public float GetCurrent() => current;
    public float GetMax() => maxCurrent;
}