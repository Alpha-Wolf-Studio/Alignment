using UnityEngine;
using UnityEngine.UI;

public class UiComponentLife : MonoBehaviour
{
    public enum TypeLife { Armor, Energy }
    [SerializeField] private TypeLife typeLife = TypeLife.Armor;
    [SerializeField] private Image filledImage;
    [SerializeField] private Character character;
    [SerializeField] private UiWorldFadeByDistance uiFade;
    private Transform focus;
    private bool enable;

    private void Awake()
    {
        focus = GameManager.Get().player.transform;
        uiFade = GetComponent<UiWorldFadeByDistance>();
    }
    void Start()
    {
        if (uiFade)
        {
            uiFade.otherTransform = focus;
            uiFade.onActive += ActiveUi;
        }
        character.OnUpdateStats += UpdateUI;
        UpdateUI();
    }
    void Update()
    {
        if (enable)
        {
            transform.LookAt(focus, Vector3.up);
        }
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
    void ActiveUi(bool on)
    {
        enable = on;
        filledImage.enabled = on;
    }
}