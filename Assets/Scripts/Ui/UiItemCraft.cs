using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UiItemCraft : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public UiCrafting uiCraft;
    [HideInInspector] public Item item;
    [HideInInspector] public int index;
    private int plusIndex = 10;
    public Image buttonCraft;
    public Image panelAvailable;
    public Image myImage;
    public TextMeshProUGUI myName;
    public TextMeshProUGUI myAmount;
    public GameObject toolTip;
    
    void Start()
    {
        uiCraft = FindObjectOfType<UiCrafting>();
        uiCraft.uiInv.OnRefreshAllButtonsEvent += Refresh;
        for (int i = 0; i < item.recipe.Count; i++)
        {
            UiItemCraft go = Instantiate(uiCraft.prefabCrafteable, toolTip.transform);
            go.item = item.recipe[i].item;
            go.uiCraft = uiCraft;
            go.index = index + 1;

            go.myAmount.text = "";
            if (item.recipe[i].amount < 10) go.myAmount.text = " ";
            go.myAmount.text = item.recipe[i].amount.ToString();
        }

        toolTip.GetComponent<Canvas>().sortingOrder = index + plusIndex;
        if (!item.crafteable)
        {
            if (buttonCraft)
                Destroy(buttonCraft.gameObject);
            if (toolTip)
                Destroy(toolTip.gameObject);
        }
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
        if (item.crafteable && buttonCraft)
        {
            if (uiCraft.craft.IsCraftPosible(item))
            {
                buttonCraft.color = Color.green;
                buttonCraft.GetComponent<Button>().interactable = true;
            }
            else
            {
                buttonCraft.color = Color.red;
                buttonCraft.GetComponent<Button>().interactable = false;
            }
        }

        if (toolTip)
            toolTip.GetComponent<Canvas>().sortingOrder = index + plusIndex;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip)
        {
            toolTip.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTip)
        {
            toolTip.SetActive(false);
        }
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
            if (uiCraft.craft.IsCraftPosible(item))
            {
                uiCraft.craft.Craft(item);
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
            if (toolTip)
                toolTip.SetActive(false);
            return "";
        }

        string text = item.ItemToString();

        // todo: traer la recursividad de public bool IsCraftPosible(Item item) que devuelve INT

        return text;
    }
}