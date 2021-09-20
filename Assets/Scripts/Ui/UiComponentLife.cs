using UnityEngine;
using UnityEngine.UI;

public class UiComponentLife : MonoBehaviour
{
    public enum TypeLife { Armor, Energy }
    [SerializeField] private TypeLife typeLife = TypeLife.Armor;

    [SerializeField] private Image filledImage;
    [SerializeField] private Character character;
    private Transform focus;

    private void Awake()
    {
        focus = GameManager.Get().player.transform;
    }
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