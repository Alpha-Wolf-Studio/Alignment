using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Armor,
    Defense,
    Energy,
    Damage,
    Walk,
    AttackSpeed,
    Stamina
}
public class CharacterStats : MonoBehaviour
{
    public int currentLevel = 1;
    public List<Stat> characterStats = new List<Stat>();

    private void Awake()
    {
        CheckDuplicate();
        InitStats();
    }
    public void InitStats()
    {
        for (int i = 0; i < characterStats.Count; i++)
        {
            characterStats[i].InitStat();
        }
    }
    public void SetDifficult()
    {
        for (int i = 0; i < currentLevel - 1; i++)
        {
            for (int j = 0; j < characterStats.Count; j++)
            {
                characterStats[j].Use();
            }
        }
    }
    public int IndexStat(StatType statType)
    {
        for (int i = 0; i < characterStats.Count; i++)
        {
            if (characterStats[i].GetStatsType() == statType)
            {
                return i;
            }
        }
        return -1;
    }
    public Stat GetStat(StatType typeStat)
    {
        int index = IndexStat(typeStat);
        if (index < 0)
            return null;
        else
            return characterStats[index];
    }
    public void CheckDuplicate()
    {
        for (int i = 0; i < characterStats.Count; i++)
        {
            StatType typeStat = characterStats[i].GetStatsType();
            for (int j = i + 1; j < characterStats.Count - 1; j++)
            {
                if (typeStat == characterStats[j].GetStatsType())
                {
                    Debug.LogWarning("Stat: " + typeStat + " Está duplicado.", gameObject);
                }
            }
        }
    }
}