using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiCrafting : MonoBehaviour
{
    public UiInventory uiInv;
    public List<int> listCraftID = new List<int>();
    public Transform panelContentCraft;
    private RectTransform panelContentCraftRt;
    public UiItemCraft uiItemCraft;
    [HideInInspector] public Crafting craft;
    public float seconsWaitToolTip = 0.5f;
    private List<TextMeshProUGUI> craftsNames = new List<TextMeshProUGUI>();

    private void Awake()
    {
        craft = GameManager.Get().player.GetComponent<Crafting>();
        panelContentCraftRt = panelContentCraft.GetComponent<RectTransform>();
    }
    void Start()
    {
        Invoke(nameof(LoadCraft), 1f);
    }
    void LoadCraft()
    {
        int lastID = ItemManager.Get().GetLastID();
        for (int id = 0; id < lastID; id++)
        {
            Item item = ItemManager.Get().GetItemFromID(id);
            if (item.crafteable)
            {
                if (ItemManager.Get().GetItemFromID(id).recipe.Count > 0)
                {
                    if (ItemManager.Get().GetItemFromID(id).recipe[0].item)
                    {
                        listCraftID.Add(id);
                        UiItemCraft craft = Instantiate(uiItemCraft, panelContentCraft.transform);
                        craft.item = ItemManager.Get().GetItemFromID(id);
                        craft.name = item.name;
                        craftsNames.Add(craft.myName);
                    }
                    else
                    {
                        Debug.LogWarning("Es crafteable y el item es nulo: " + ItemManager.Get().GetItemFromID(id).itemName);
                    }
                }
                else
                {
                    Debug.LogWarning("Es crafteable y no tiene items: " + ItemManager.Get().GetItemFromID(id).itemName);
                }
            }
        }
        ResizeContent();
    } 
    void ResizeContent()
    {
        GridLayoutGroup grid = panelContentCraft.GetComponent<GridLayoutGroup>();
        int cantChild = panelContentCraft.transform.childCount;
        int columns = 2;
        int height = 50;
        int spacing = 10;

        while (cantChild % columns != 0)
        {
            cantChild++;
        }

        int maxHeights = (cantChild / columns) * height;
        int maxSpacings = ((cantChild / columns) - 1) * spacing;

        panelContentCraftRt.sizeDelta = new Vector2(panelContentCraftRt.sizeDelta.x, maxHeights + maxSpacings);
        Invoke(nameof(SetSameSize), 0.1f);
    }
    public void SetSameSize()
    {
        float minSize = 999;
        for (int i = 0; i < craftsNames.Count; i++)
        {
            if (craftsNames[i].fontSize < minSize)
                minSize = craftsNames[i].fontSize;
        }
        for (int i = 0; i < craftsNames.Count; i++)
        {
            craftsNames[i].enableAutoSizing = false;
            craftsNames[i].fontSize = minSize;
        }
    }
}