using System;
using System.Collections;
using System.Collections.Generic;
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
    private Character character;
    private Inventory inventory;
    public Image[] hud;

    private bool godMode;

    private float cheatArmor = 50;
    private float cheatEnergy = 50;
    private float cheatDamage = 10;
    private float cheatSpeed = 0.01f;

    private bool forceScriptDisable = true;
    private void Awake()
    {
        player = GameManager.Get().player;
        character = player.GetComponent<Character>();
        inventory = player.GetComponent<Inventory>();
    }
    void Start()
    {
        if (cheatEnable)
        {
            //CheatEnable();
        }
    }
    void Update()
    {
        if(!forceScriptDisable)
            if (!cheatEnable)
            {
                if (Input.GetKeyDown(KeyCode.Keypad0))
                {
                    activeSecuence = true;
                    if (onTime < maxTimeDelay)
                    {
                        count++;
                        if (count >= maxCount)
                        {
                            //CheatEnable();
                        }
                    }
                }

                if (activeSecuence)
                    onTime += Time.deltaTime;

                if (onTime > maxTimeDelay)
                {
                    activeSecuence = false;
                    count = 0;
                    onTime = 0;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    Debug.Log("Cheated Armor: " + cheatArmor);
                    CheatArmor();
                }
                else if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    Debug.Log("Cheated Energy: " + cheatEnergy);
                    CheatEnergy();
                }
                else if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    Debug.Log("Cheated Damage: " + cheatDamage);
                    CheatDamage();
                }
                else if (Input.GetKeyDown(KeyCode.Keypad4))
                {
                    Debug.Log("GodMode Deshabilitado.");
                }
                else if (Input.GetKeyDown(KeyCode.Keypad5))
                {
                    player.AddSpeed(cheatSpeed);
                    Debug.Log("Current Speed: " + character.GetSpeed());
                }
                else if (Input.GetKeyDown(KeyCode.Keypad6))
                {

                }
                else if (Input.GetKeyDown(KeyCode.Keypad7))
                {

                }
                else if (Input.GetKeyDown(KeyCode.Keypad8))
                {

                }
            }
    }
    void SuscribeInfinityCheat(Stat stat)
    {
        switch (stat.name)
        {
            case "Energy":
                character.OnUpdateStats += CheatEnergy;
                break;
            case "Armor":            // todo seguir acá
                break;
            case "Attack":           // todo seguir acá
                break;
            default:                 // todo seguir acá
                break;
        }
    }
    public void CheatArmor()
    {
        character.TopCurrentArmor();
    }
    public void CheatEnergy()
    {
        character.TopCurrentEnergy();
    }
    public void CheatDamage()
    {
        character.TopCurrentDamage();
    }
    public void CheatEnable(Character.Stats stats)
    {
        cheatEnable = true;
        onCheat?.Invoke();
        Debug.Log("Cheat Enabled: " + stats.ToString());
        PrintDataCheat();
        for (int i = 0; i < hud.Length; i++)
        {
            if (hud[i])
                hud[i].color = Color.red;
        }

        switch (stats)
        {
            case Character.Stats.Armor:
                character.OnUpdateStats += CheatArmor;
                break;
            case Character.Stats.Damage:
                character.OnUpdateStats += CheatDamage;
                break;
            case Character.Stats.Defense:
                //character.OnUpdateStats += CheatDefense;
                break;
            case Character.Stats.Energy:
                character.OnUpdateStats += CheatEnergy;
                break;
            case Character.Stats.Speed:
                //character.OnUpdateStats += CheatSpeed;
                break;
        }
    }
    public void PrintDataCheat()
    {
        Debug.Log("1- Cheat Armor: " + cheatArmor);
        Debug.Log("2- Cheat Energy: " + cheatEnergy);
        Debug.Log("3- Cheat Damage: " + cheatDamage);
        Debug.Log("4- Cheat GodMode: OnOff.");
        Debug.Log("5- Cheat Speed: " + cheatSpeed.ToString("F2") + " DANGER");
    }
    public void Regenerate()
    {
        character.AddCurrentArmor(100);
        character.AddCurrentEnergy(100);
    }
    public void RegenerateStat(ref Stat stat)
    {
        stat.AddCurrent(999);
    }
}