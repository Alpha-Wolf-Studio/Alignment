using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiItemRepair : MonoBehaviour, IInteractuable
{
    [HideInInspector] public Inventory inv;
    public int index;
    public Image image;
    public TextMeshProUGUI text;
    public ReparableObject obj;
    private int restAmount = 1;

    public void Interact()
    {
        Item item = ItemManager.GetInstance().GetItemFromID(obj.idRequired[index].ID);
        if (inv.CheckForItem(item, restAmount))
        {
            if (obj.idRequired[index].amount > 0)
            {
                inv.RemoveItem(item, restAmount);
                obj.idRequired[index].amount -= restAmount;
                Debug.Log("Depositado.");
                obj.RefreshUI(this, index);
            }
            else
                Debug.Log("Ya ta Full.");
        }
        Debug.Log("No hay items Bro.");

    }
    public void Visible()
    {
        
    }
    
}