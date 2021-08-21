using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiComponentLife : MonoBehaviour
{
    public Image filledImageEnergy;
    public Image filledImageArmor;
    public Character character;
    [Tooltip("Donde hace Focus el canvas, Recomendado: Player.")]
    public Transform focus;

    void Start()
    {
        character.OnTakeDamage += TakeDamage;
        filledImageEnergy.fillAmount = character.GetEnergy() / character.GetStartedEnergy();
        //filledImageArmor.fillAmount = character.GetArmor() / character.GetStartedArmor();
    }
    void Update()
    {
        transform.LookAt(focus, Vector3.up);
    }
    void TakeDamage()
    {
        filledImageEnergy.fillAmount = character.GetEnergy() / character.GetStartedEnergy();
        //filledImageArmor.fillAmount = character.GetArmor() / character.GetStartedArmor();
    }
}