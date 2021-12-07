using System;
using UnityEngine;
using UnityEngine.UI;
public class ModuleCheat : MonoBehaviour
{
    public Action onCheat;
    public bool cheatEnable;
    private bool activeSecuence;

    public float maxTimeDelay = 1.5f;
    private float onTime;

    public int maxCount = 5;
    private int count;

    public PlayerController player;
    public Image[] hud;
    
    private void Awake()
    {
        player = GameManager.Get().player;
    }
    void SuscribeInfinityCheat(Stat stat)
    {
        stat.modifyValues = !stat.modifyValues;
        stat.SetToMax();
    }
    public void CheatEnable(Stat stat)
    {
        Debug.Log("Cheat Enabled: " + stat.GetStatsType());
        if (!cheatEnable)
        {
            onCheat?.Invoke();
            for (int i = 0; i < hud.Length; i++)
            {
                if (hud[i])
                    hud[i].color = Color.red;
            }
            cheatEnable = true;
        }
        SuscribeInfinityCheat(stat);
    }
}