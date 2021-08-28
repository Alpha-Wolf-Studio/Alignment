using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public ReparableObject reparableObject;

    private bool godMode;


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
                        cheatEnable = true;
                        onCheat?.Invoke();
                        Debug.Log("Cheat Enabled.");
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
                Debug.Log("Cheated Armor: " + 50);
                character.TakeArmorDamage(-50);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Debug.Log("Cheated Energy: " + 50);
                character.AddCurrentEnergy(50);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Debug.Log("Cheated Damage: " + 10);
                character.AddCurrentAttack(10);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (!godMode)
                {
                    godMode = true;
                    character.OnTakeDamage += Regenerate;
                    Debug.Log("GodMode: Activated");
                }
                else
                {
                    godMode = false;
                    character.OnTakeDamage -= Regenerate;
                    Debug.Log("GodMode: Desactivated");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                player.AddSpeed(0.1f);
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

    void Regenerate()
    {
        character.AddCurrentArmor(100);
    }
}
