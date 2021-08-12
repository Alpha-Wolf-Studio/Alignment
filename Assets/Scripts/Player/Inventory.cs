﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Slot> currentItems;
    [SerializeField] int size = 10;

    private void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            Slot newSlot = new Slot();
            currentItems.Add(newSlot);
        }
    }

    public void SetNewInventory(List<Slot> newInventory)
    {
        currentItems.Clear();
        foreach (Slot slot in newInventory)
        {
            currentItems.Add(slot);
        }
    }

    public bool AddNewItem(int ID, int amount, int slotPos)
    {
        if (currentItems[slotPos].IsEmpty())
        {
            currentItems[slotPos].FillSlot(ID, amount);
            return true;
        }
        else
        {
            if (ID == currentItems[slotPos].ID && ItemManager.GetInstance().GetItemFromID(ID).maxStack >= currentItems[slotPos].amount + amount)
            {
                currentItems[slotPos].AddAmount(amount);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool AddNewItem(int ID, int amount)
    {
        for (int i = 0; i < size; i++)
        {
            if (currentItems[i].IsEmpty())
            {
                currentItems[i].FillSlot(ID, amount);
                return true;
            }
        }
        return false;
    }

    public void DeleteItem(int slotPos)
    {
        if (!currentItems[slotPos].IsEmpty())
        {
            currentItems[slotPos].EmptySlot();
        }
    }

    public void SwapItem(int slotPosFrom, int slotPosTo)
    {
        if (slotPosFrom == slotPosTo) return;
        if (!currentItems[slotPosFrom].IsEmpty() && !currentItems[slotPosTo].IsEmpty())
        {
            Item fromItem = ItemManager.GetInstance().GetItemFromID(currentItems[slotPosFrom].ID);
            Item toItem = ItemManager.GetInstance().GetItemFromID(currentItems[slotPosTo].ID);
            if (toItem.maxStack > 1 && fromItem.maxStack > 1)
            {
                currentItems[slotPosFrom].amount = currentItems[slotPosTo].AddAmount(currentItems[slotPosFrom].amount);
                if (currentItems[slotPosFrom].amount <= 0)
                {
                    currentItems[slotPosFrom].EmptySlot();
                }
                return;
            }
        }
        Slot temp = new Slot(currentItems[slotPosFrom].ID, currentItems[slotPosFrom].amount);
        currentItems[slotPosFrom] = currentItems[slotPosTo];
        currentItems[slotPosTo] = temp;
    }

    public bool UseItem(int slotPos)    // Doble click o Click Derecho
    {
        if (ItemManager.GetInstance().GetItemFromID(currentItems[slotPos].ID).consumible)
        {
            currentItems[slotPos].AddAmount(-1);
            if (currentItems[slotPos].IsEmpty())
                return false;
        }
        return true;
    }

    public void Divide(int slotPos)
    {
        if (currentItems[slotPos].amount > 1)
        {
            int dividedAmount = (currentItems[slotPos].amount / 2);
            if (currentItems[slotPos].amount % 2 != 0) dividedAmount++;
            if (AddNewItem(currentItems[slotPos].ID, dividedAmount))
            {
                currentItems[slotPos].amount /= 2;
            }
        }
    }

    public void Sort()
    {
        currentItems.Sort();
    }

    public int GetSize()
    {
        return size;
    }
    public Slot GetSlot(int index)
    {
        return currentItems[index];
    }
    public void SetSlot(int index, Slot slot)
    {
        currentItems[index] = slot;
    }
    public int GetID(int index)
    {
        return currentItems[index].ID;
    }
    public List<Slot> GetInventoryList()
    {
        return currentItems;
    }
}
