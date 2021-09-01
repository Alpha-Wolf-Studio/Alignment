using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiComponentLife : MonoBehaviour
{
    public enum TypeLife { Armor, Energy }
    public TypeLife typeLife = TypeLife.Armor;

    public Image filledImage;
    public Character character;
    [Tooltip("Donde hace Focus el canvas, Recomendado: Player.")]
    public Transform focus;

    void Start()
    {
        character.OnUpdateStats += UpdateUI;
        UpdateUI();
    }
    void Update()
    {
        transform.LookAt(focus, Vector3.up);
    }
    void UpdateUI()
    {
        switch (typeLife)
        {
            case TypeLife.Armor:
                filledImage.fillAmount = character.GetArmor().GetCurrent() / character.GetArmor().GetMax();
                break;
            case TypeLife.Energy:
                filledImage.fillAmount = character.GetEnergy().GetCurrent() / character.GetEnergy().GetMax();
                break;
            default:
                break;
        }
    }
}