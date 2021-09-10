using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiCrafting : MonoBehaviour
{
    public UiInventory uiInv;
    public List<int> listCraftID = new List<int>();
    public Transform panelCraft;
    public UiItemCraft prefabCrafteable;
    public Crafting craft;
    
    public RectTransform contentCraft;
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
                        UiItemCraft craft = Instantiate(prefabCrafteable, panelCraft.transform);
                        craft.item = ItemManager.GetInstance().GetItemFromID(id);
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
        //ResizeContent();
    }
    void ResizeContent()
    {
        int cantChild = contentCraft.transform.childCount;
        GridLayoutGroup grid = contentCraft.GetComponent<GridLayoutGroup>();

        float cellSize = grid.cellSize.y;
        cellSize += grid.spacing.y;
        int columns = grid.constraintCount;

        int currentColumn = 0;
        while (cantChild % columns != 0)
        {
            cantChild++;
            currentColumn++;
            if (currentColumn > columns)
            {
                Debug.LogError("Supera el Maximo de Columnas ", gameObject);
                break; // Salida de de emergencia de While
            }
        }
        int padding = grid.padding.bottom + grid.padding.top;

        contentCraft.sizeDelta = new Vector2(contentCraft.sizeDelta.x, cantChild * cellSize / columns + padding);
    }
}