using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UiItemCraft : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UiCrafting uiCraft;
    [HideInInspector] public Item item;

    public Button buttonCraft;
    public Image panelAvailable;
    public Image myImage;
    public TextMeshProUGUI myName;
    public TextMeshProUGUI myAmount;
    public RectTransform toolTip;
    
    void Start()
    {
        uiCraft = FindObjectOfType<UiCrafting>();
        for (int i = 0; i < item.recipe.Count; i++)
        {
            UiItemCraft go = Instantiate(uiCraft.prefabCrafteable, toolTip);
            go.item = item.recipe[i].item;
            go.uiCraft = uiCraft;
        }

        if (!item.crafteable) Destroy(buttonCraft.gameObject);
        myImage.sprite = item.icon;
        myName.text = item.name;
        Refresh();
    }
    private void OnEnable()
    {
        if (item)
            Refresh();
    }
    public void Refresh()
    {
        if (item.crafteable)
        {
            buttonCraft.interactable = uiCraft.craft.IsCraftPosible(item);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item.id > 0)
        {
            toolTip.gameObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.gameObject.SetActive(false);
    }
    public void TryCraftButton()
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
            Debug.Log("Click sobre: " + item.name);
            if (uiCraft.craft.Craft(item))
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
        
        for (int i = 0; i < item.recipe.Count; i++)
        {
            if (item.recipe[i].amount < 10) text += " ";
            text += item.recipe[i].amount + "  " + item.recipe[i].item.name;
            text += "\n";
        }

        //toolTipText.text = text;
    }
    string TextFormatter()
    {
        if (item.id < 0)
        {
            toolTip.gameObject.SetActive(false);
            return "";
        }

        string text = item.ItemToString();

        // todo: traer la recursividad de public bool IsCraftPosible(Item item) que devuelve INT

        return text;
    }
}