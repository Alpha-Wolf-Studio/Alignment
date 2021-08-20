﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReparableObject : MonoBehaviour
{
    public string nameToRepair;
    public TextMeshProUGUI nameToRepairTM;
    public List<Slot> idRequired = new List<Slot>();
    public RectTransform panelObjects;
    public Transform player;
    public GameObject pfItem;
    public float maxDistanceToShow;
    public float minDistanceToShow;
    public CanvasGroup canvasGroup;
    private bool enable;
    void Start()
    {
        LoadItemsRequired();
    }
    void Update()
    {
        EnablePanel();
    }

    void LoadItemsRequired()
    {
        nameToRepairTM.text = nameToRepair;
        for (int i = 0; i < idRequired.Count; i++)
        {
            UiItemRepair itemReq = Instantiate(pfItem, panelObjects).GetComponent<UiItemRepair>();
            itemReq.image.sprite = ItemManager.GetInstance().GetItemFromID(idRequired[i].ID).icon;
            RefreshUI(itemReq, i);
            itemReq.index = i;
            itemReq.obj = this;
            itemReq.inv = player.GetComponent<Inventory>();
        }
    }
    public void RefreshUI(UiItemRepair item, int index)
    {
        item.text.text = ItemManager.GetInstance().GetItemFromID(idRequired[index].ID).itemName; 
        item.text.text += "\nRemaining: " + idRequired[index].amount;
    }
    void EnablePanel()
    {
        float distanceSqr = Vector3.SqrMagnitude(player.position - transform.position);
        if (distanceSqr < maxDistanceToShow * maxDistanceToShow)
        {
            float distance = DistanceToPlayer();
            canvasGroup.alpha = 1 - (distance - minDistanceToShow) / (maxDistanceToShow - minDistanceToShow);
            if (!enable)
            {
                Enabled(true);
            }
        }
        else
        {
            if (enable)
            {
                Enabled(false);
            }
        }
    }
    float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }
    void Enabled(bool on)
    {
        enable = on;
        canvasGroup.blocksRaycasts = on;
        canvasGroup.interactable = on;
    }
}