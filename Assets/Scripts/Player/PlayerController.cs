﻿using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Entity))]
public class PlayerController : MonoBehaviour
{
    public Action onInventory;
    public Action<bool> onPause;

    private PlayerState[] states;
    [HideInInspector] public PlayerGame playerGame;
    [HideInInspector] public PlayerPause playerPause;
    [HideInInspector] public PlayerInventory playerInventory;
    private Entity entity;

    [Header("Movement")]
    [SerializeField] private float verticalSensitive = 2;
    [SerializeField] private float horizontalSensitive = 2;
    [SerializeField] private int minCameraClampVertical = -50;
    [SerializeField] private int maxCameraClampVertical = 50;
    [SerializeField] private float forceJump;
    [SerializeField] private float forceFly;
    
    [Space (10)]
    public float maxCoolDownShoot;
    public float currentCoolDownShoot = 10;

    private float energySpendRun = 0.2f;
    private float energyRegenerate = 0.05f;
    
    public bool IsJetpack { get; set; }
    public bool IsGrounded{ get; set; }

    private void Awake()
    {
        entity = GetComponent<Entity>();
        playerGame = GetComponent<PlayerGame>();
        playerPause = GetComponent<PlayerPause>();
        playerInventory = GetComponent<PlayerInventory>();
        states = GetComponentsInChildren<PlayerState>();
    }
    void Start()
    {
        AvailableCursor(false);
        entity.OnEntityTakeDamage += PlayerTakeDamage;

        if (DataPersistant.Get())
        {
            DataPersistant.Get().playerController = this;
            Vector2 sensitive = new Vector2();
            sensitive = DataPersistant.Get().gameSettings.controls.GetSensitives();
            SetSensitive(sensitive);
        }
    }
    private void Update()
    {
        UpdateCoolDown();
        UpdateStamina();
    }
    void UpdateCoolDown()
    {
        if (currentCoolDownShoot < maxCoolDownShoot)
            currentCoolDownShoot += Time.deltaTime;
    }
    void UpdateStamina()
    {
        if (!playerGame.useEnergyRun)
        {
            if (GetCurrentStamina() < GetMaxStamina())
            {
                if (IsGrounded)
                {
                    entity.entityStats.GetStat(StatType.Stamina).AddCurrent(energyRegenerate * Time.deltaTime);
                }
            }
        }
    }
    public void TryOnpenInventory()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            onInventory?.Invoke();
        }
    }
    public void TryPause(bool enablePause)
    {
        if (Input.GetButtonDown("Cancel"))
        {
            onPause?.Invoke(enablePause);
        }
    }
    public void SetSensitive(Vector2 sensitive)
    {
        horizontalSensitive = sensitive.x;
        verticalSensitive = sensitive.y;
        Debug.Log("Sensitive X: " + horizontalSensitive + "Sensitive Y: " + verticalSensitive);
    }
    public void AvailableCursor(bool enable)
    {
        if (enable)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public bool DamageOnShoot() => currentCoolDownShoot < maxCoolDownShoot;
    public void ChangeControllerToGame() => ChangeController(playerGame);
    public void ChangeControllerToPause() => ChangeController(playerPause);
    public void ChangeControllerToInventory()=> ChangeController(playerInventory);
    public void ChangeControllerToNone() => ChangeController(null);
    private void ChangeController(PlayerState currentState)
    {
        foreach (var state in states)
        {
            if (currentState != state)
                state.enabled = false;
        }

        if (currentState && !currentState.enabled)
            currentState.enabled = true;
    }
    private void OnCollisionEnter(Collision other)
    {
        if(Global.LayerEquals(LayerMask.GetMask("Ground"), other.gameObject.layer))
        {
            IsGrounded = true;
        }
    }
    public float GetCurrentStamina() => entity.entityStats.GetStat(StatType.Stamina).GetCurrent();
    public float GetMaxStamina() => entity.entityStats.GetStat(StatType.Stamina).GetMax();
    public void AddSpeed(float increaseIn)
    {
        entity.entityStats.GetStat(StatType.Walk).AddCurrent(increaseIn);
    }
    void PlayerTakeDamage(DamageInfo info)
    {
        if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerEnergyDamage))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerEnergyDamage), gameObject);
    }

    public Vector2 GetSensitives() => new Vector2(horizontalSensitive, verticalSensitive);
    public Vector2 GetCameraClamp() => new Vector2(minCameraClampVertical, maxCameraClampVertical);
    public float GetForceFly() => forceFly;
    public float GetForceJump() => forceJump;
    public float GetEnergySpendRun() => energySpendRun;
}