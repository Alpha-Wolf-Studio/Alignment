using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiItemCraft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UiCrafting uiCraft;
    public int id = 0;
    public int index = 0;
    public Color colorAvailable = Color.green;
    public Color colorDisable = Color.red;
    public float alphaColor = 70;
    public Image panelAvailable;
    private RectTransform rectTransform;
    public Image myImage;

    private void Awake()
    {
        colorAvailable.a = alphaColor;
        colorDisable.a = alphaColor;

        uiCraft = FindObjectOfType<UiCrafting>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {

    }
    void Update()
    {
        
    }
    public void SetButton(int id)
    {
        this.id = id;
        GetComponent<Image>().sprite = ItemManager.GetInstance().GetItemFromID(id).icon;

        if (uiCraft.craft.IsCraftPosible(ItemManager.GetInstance().GetItemFromID(id)))
        {
            panelAvailable.color = colorAvailable;
        }
        else
        {
            panelAvailable.color = colorDisable;
        }

        Refresh();
    }
    public void Refresh()
    {
        myImage.sprite = ItemManager.GetInstance().GetItemFromID(id).icon;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (id > 0)
        {
            uiCraft.toolTip.gameObject.SetActive(true);
            uiCraft.RefreshToolTip(rectTransform);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + gameObject.name, gameObject);

        
        uiCraft.toolTip.gameObject.SetActive(false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        uiCraft.toolTip.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // Todos los posibles
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // De a 5
        }
        else
        {
            Debug.Log("Click sobre: " + ItemManager.GetInstance().GetItemFromID(id).itemName);
            if (uiCraft.craft.Craft(ItemManager.GetInstance().GetItemFromID(id)))
            {
                Debug.Log("Craft Exitoso.");
                uiCraft.uiInv.RefreshAllButtons();
            }
            else
            {
                Debug.Log("Craft Fracaso.");
            }
        }
    }
}