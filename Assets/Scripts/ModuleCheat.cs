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
    public ReparableObject reparableObject;

    private bool godMode;

    private float cheatArmor = 50;
    private float cheatEnergy = 50;
    private float cheatDamage = 10;
    private float cheatSpeed = 0.01f;

    private void Awake()
    {
        character = player.GetComponent<Character>();
        inventory = player.GetComponent<Inventory>();
    }
    void Start()
    {

    }
    void Update()
    {
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
                        CheatEnable();
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
                character.AddCurrentArmor(cheatArmor);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Debug.Log("Cheated Energy: " + cheatEnergy);
                character.AddCurrentEnergy(cheatEnergy);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Debug.Log("Cheated Damage: " + cheatDamage);
                character.AddCurrentAttack(cheatDamage);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (!godMode)
                {
                    godMode = true;
                    character.OnUpdateStats += Regenerate;
                    Debug.Log("GodMode: Activated");
                }
                else
                {
                    godMode = false;
                    character.OnUpdateStats -= Regenerate;
                    Debug.Log("GodMode: Desactivated");
                }
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
    void CheatEnable()
    {
        cheatEnable = true;
        onCheat?.Invoke();
        Debug.Log("Cheat Enabled.");
        PrintDataCheat();
        for (int i = 0; i < hud.Length; i++)
        {
            hud[i].color = Color.red;
        }
    }
    void PrintDataCheat()
    {
        Debug.Log("1- Cheat Armor: " + cheatArmor);
        Debug.Log("2- Cheat Energy: " + cheatEnergy);
        Debug.Log("3- Cheat Damage: " + cheatDamage);
        Debug.Log("4- Cheat GodMode: OnOff.");
        Debug.Log("5- Cheat Speed: " + cheatSpeed.ToString("F2") + " DANGER");
    }
    void Regenerate()
    {
        character.AddCurrentArmor(100);
    }
}