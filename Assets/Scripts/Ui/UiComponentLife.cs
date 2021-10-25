using UnityEngine;
using UnityEngine.UI;

public class UiComponentLife : MonoBehaviour
{
    public enum TypeLife { Armor, Energy }
    [SerializeField] private TypeLife typeLife = TypeLife.Armor;
    [SerializeField] private Image filledImage;
    [SerializeField] private Entity character;
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
                float currentArmor = character.entityStats.GetStat(StatType.Armor).GetCurrent();
                float maxArmor = character.entityStats.GetStat(StatType.Armor).GetMax();
                filledImage.fillAmount = currentArmor / maxArmor;
                break;
            case TypeLife.Energy:
                float currentEnergy = character.entityStats.GetStat(StatType.Armor).GetCurrent();
                float maxEnergy = character.entityStats.GetStat(StatType.Armor).GetMax();
                filledImage.fillAmount = currentEnergy / maxEnergy;
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