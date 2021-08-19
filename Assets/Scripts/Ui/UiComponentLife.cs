using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiComponentLife : MonoBehaviour
{
    public Image filledImage;
    public Character character;
    [Tooltip("Donde hace Focus el canvas, Recomendado: Player.")]
    public Transform focus;

    void Start()
    {
        character.OnTakeDamage += TakeDamage;
        filledImage.fillAmount = character.GetEnergy() / character.GetStartedEnergy();
    }
    void Update()
    {
        transform.LookAt(focus, Vector3.up);
    }
    void TakeDamage()
    {
        filledImage.fillAmount = character.GetEnergy() / character.GetStartedEnergy();
    }
}