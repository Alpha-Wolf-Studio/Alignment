using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiItemCraft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UiCrafting uiCraft;
    public int id;
    public int index;
    private Color colorAvailable = Color.white;
    private Color colorDisable = Color.red;
    private RectTransform rectTransform;

    public Image panelAvailable;
    public Image myImage;
    public TextMeshProUGUI myName;
    public TextMeshProUGUI myAmount;
    public RectTransform toolTip;
    public TextMeshProUGUI toolTipText;

    private void Awake()
    {
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
        myImage.sprite = ItemManager.GetInstance().GetItemFromID(id).icon;

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
        myName.text = ItemManager.GetInstance().GetItemFromID(id).name;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (id > 0)
        {
            toolTip.gameObject.SetActive(true);
            RefreshToolTip();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown: " + gameObject.name, gameObject);
        toolTip.gameObject.SetActive(false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.gameObject.SetActive(false);
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
    public void RefreshToolTip()
    {
        string text = "";
        Item item = ItemManager.GetInstance().GetItemFromID(id);
        
        for (int i = 0; i < item.recipe.Count; i++)
        {
            if (item.recipe[i].amount < 10) text += " ";
            text += item.recipe[i].amount + "  " + item.recipe[i].item.name;
            text += "\n";
        }

        toolTipText.text = text;
    }
    string TextFormatter()
    {
        if (id < 0)
        {
            toolTip.gameObject.SetActive(false);
            return "";
        }
        Item myItem = ItemManager.GetInstance().GetItemFromID(id);

        string text = myItem.ItemToString();

        // todo: traer la recursividad de public bool IsCraftPosible(Item item) que devuelve INT

        return text;
    }
}