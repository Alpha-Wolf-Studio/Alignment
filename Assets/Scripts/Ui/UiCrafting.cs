using System.Collections.Generic;
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
        int lastID = ItemManager.GetInstance().GetLastID();
        for (int id = 0; id < lastID; id++)
        {
            Item item = ItemManager.GetInstance().GetItemFromID(id);
            if (item.crafteable)
            {
                if (ItemManager.GetInstance().GetItemFromID(id).recipe.Count > 0)
                {
                    if (ItemManager.GetInstance().GetItemFromID(id).recipe[0].item)
                    {
                        listCraftID.Add(id);
                        UiItemCraft craft = Instantiate(uiItemCraft, panelContentCraft.transform);
                        craft.item = ItemManager.GetInstance().GetItemFromID(id);
                        craft.name = item.name;
                    }
                    else
                    {
                        Debug.LogWarning("Es crafteable y el item es nulo: " + ItemManager.GetInstance().GetItemFromID(id).itemName);
                    }
                }
                else
                {
                    Debug.LogWarning("Es crafteable y no tiene items: " + ItemManager.GetInstance().GetItemFromID(id).itemName);
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
    }
}