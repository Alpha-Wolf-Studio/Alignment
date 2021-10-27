using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReparableObject : MonoBehaviour
{                       // Todo: Solución: Separar los calculos y los requerimientos de lo que muestra el Canvas
    public Action<RepairLocations> OnRepair;
    public RepairLocations location;
    public string nameToRepair;
    public TextMeshProUGUI nameToRepairTM;
    public List<Slot> idRequired = new List<Slot>();
    public RectTransform panelObjects;
    public Transform player;
    public GameObject pfItem;
    private List<BoxCollider> boxColliders = new List<BoxCollider>();
    [SerializeField] private UiWorldFadeByDistance uiFade;

    void Start()
    {
        LoadItemsRequired();
        if (uiFade)
            uiFade.onActive += ActiveColliders;
    }
    void LoadItemsRequired()
    {
        nameToRepairTM.text = nameToRepair;
        for (int i = 0; i < idRequired.Count; i++)
        {
            UiItemRepair itemReq = Instantiate(pfItem, panelObjects).GetComponent<UiItemRepair>();
            boxColliders.Add(itemReq.GetComponent<BoxCollider>());
            itemReq.image.sprite = ItemManager.Get().GetItemFromID(idRequired[i].ID).icon;
            RefreshUI(itemReq, i);
            itemReq.index = i;
            itemReq.obj = this;
            itemReq.inv = player.GetComponent<Inventory>();
        }
    }
    public void RefreshUI(UiItemRepair item, int index)
    {
        item.text.text = ItemManager.Get().GetItemFromID(idRequired[index].ID).itemName; 
        item.text.text += "\nRemaining: " + idRequired[index].amount;
        if (CheckRepaired())
        {
            OnRepair?.Invoke(location);
        }
    }
    bool CheckRepaired()
    {
        foreach (Slot slot in idRequired)
        {
            if (slot.amount > 0)
                return false;
        }
        return true;
    }
    void ActiveColliders(bool on)
    {
        foreach (var coll in boxColliders)
        {
            coll.enabled = on;
        }
    }
}