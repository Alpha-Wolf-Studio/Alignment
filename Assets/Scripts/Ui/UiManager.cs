using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Image filledImageCoolDown;
    public Image filledImageEnergy;
    public Color CoolDownReloading = Color.blue;
    public Character character;
    public PlayerController player;
    public RectTransform sightHud;
    private float speed = 0.1f;
    private float onTime;
    void Start()
    {
        character.OnTakeDamage += TakeDamage;
        character.OnDeath += Death;
        player.onShoot+= Shoot;
    }
    void Update()
    {
        sightHud.Rotate(Vector3.forward * speed);
    }
    void TakeDamage()
    {
        filledImageEnergy.fillAmount = character.GetEnergy() / character.GetStartedEnergy();
    }
    void Shoot(float maxCoolDown, bool reloading)
    {
        if (reloading)
        {
            StartCoroutine(Reloading(maxCoolDown));
        }
    }
    void Death()
    {

    }
    IEnumerator Reloading(float maxCD)
    {
        filledImageCoolDown.color = CoolDownReloading;
        while (onTime < maxCD)
        {
            onTime += Time.deltaTime;
            filledImageCoolDown.fillAmount = onTime / maxCD;
            yield return null;
        }

        filledImageCoolDown.color = Color.white;
        onTime = 0;
    }
}