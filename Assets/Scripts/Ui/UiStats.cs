using System;
using TMPro;
using UnityEngine;
public class UiStats : MonoBehaviour
{
    public PlayerController playerController;
    private CharacterStats playerStats;
    public TextMeshProUGUI textArmor;
    public TextMeshProUGUI textDamage;
    public TextMeshProUGUI textEnergy;
    public TextMeshProUGUI textDefense;
    public TextMeshProUGUI textSpeed;

    private void Start()
    {
        playerStats = playerController.GetComponent<CharacterStats>();
        playerController.onInventory += OnInventory;
    }
    public void OnInventory()
    {
        textArmor.text = playerStats.GetStat(StatType.Armor).GetCurrent().ToString("F0");
        textDamage.text = playerStats.GetStat(StatType.Damage).GetCurrent().ToString("F0");
        textEnergy.text = playerStats.GetStat(StatType.Energy).GetCurrent().ToString("F0");
        textDefense.text = playerStats.GetStat(StatType.Defense).GetCurrent().ToString("F0");
        textSpeed.text = (playerStats.GetStat(StatType.Walk).GetCurrent() * 100).ToString("F0");
    }
}