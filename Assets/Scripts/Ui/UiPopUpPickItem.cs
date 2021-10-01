using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiPopUpPickItem : MonoBehaviour
{
    private Image myImage;
    public Image imageItem;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textAmount;
    private float ontime;
    [HideInInspector] public float maxTime;
    private UiPopUps uiPopUp;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }
    public void SetValues(Sprite image, string nameItem, string amountItem, UiPopUps uiPopUps)
    {
        uiPopUp = uiPopUps;
        imageItem.sprite = image;
        textName.text = nameItem;
        textAmount.text = amountItem;
    }
    private void Update()
    {
        ontime += Time.deltaTime;
        myImage.color = Color.Lerp(Color.white, Color.clear, ontime / maxTime);
        if (ontime > maxTime)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        uiPopUp.uiPopList.Remove(this);
    }
}