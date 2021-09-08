﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class Inventory : MonoBehaviour
{
    [SerializeField] List<Slot> currentSlots;
    [SerializeField] int size = 10;
    [SerializeField] float explosionStrenght = 300f;

    [Header("Pick Up")]
    [SerializeField] bool canPickUpItems = false;
    [SerializeField] float pickUpDistance = 1f;
    [SerializeField] LayerMask pickUpMask = default;

    public Action<Item, int> OnPickUp;

    Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
        for (int i = 0; i < size; i++)
        {
            Slot newSlot = new Slot();
            currentSlots.Add(newSlot);
        }
    }
    private void Start()
    {
        StartCoroutine(PickUpCoroutine());
    }
    public void SetNewInventory(List<Slot> newInventory)
    {
        currentSlots.Clear();
        foreach (Slot slot in newInventory)
        {
            currentSlots.Add(slot);
        }
    }

    public bool AddNewItem(int ID, int amount, int slotPos)
    {
        if (currentSlots[slotPos].IsEmpty())
        {
            currentSlots[slotPos].FillSlot(ID, amount);
            return true;
        }
        else
        {
            if (ID == currentSlots[slotPos].ID && ItemManager.GetInstance().GetItemFromID(ID).maxStack >= currentSlots[slotPos].amount + amount)
            {
                currentSlots[slotPos].AddAmount(amount);
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
        if(CanItemBeAdded(ID, amount))
        {
            int remainingAmount = amount;
            int maxAmountPerSlot = ItemManager.GetInstance().GetItemFromID(ID).maxStack;
            for (int i = 0; i < currentSlots.Count; i++)
            {
                if (currentSlots[i].ID == ID)
                {
                    currentSlots[i].amount += remainingAmount;
                    if (currentSlots[i].amount <= maxAmountPerSlot)
                    {
                        return true;
                    }
                    else
                    {
                        remainingAmount = currentSlots[i].amount - maxAmountPerSlot;
                        currentSlots[i].amount = maxAmountPerSlot;
                    }
                }
            }
            if(remainingAmount > 0)
            {
                for (int i = 0; i < currentSlots.Count; i++)
                {
                    if (currentSlots[i].IsEmpty())
                    {
                        currentSlots[i].FillSlot(ID, remainingAmount);
                        return true;
                    }
                }
            }

        }
        return false;
    }
    public bool AddNewItemInEmptySlot(int ID, int amount)
    {
        for (int i = 0; i < currentSlots.Count; i++)
        {
            if (currentSlots[i].IsEmpty())
            {
                currentSlots[i].FillSlot(ID, amount);
                return true;
            }
        }
        return false;
    }
    public void DeleteItem(int slotPos)
    {
        if (!currentSlots[slotPos].IsEmpty())
        {
            currentSlots[slotPos].EmptySlot();
        }
    }
    public void SwapItem(int slotPosFrom, int slotPosTo)
    {
        if (slotPosFrom == slotPosTo) return;
        if (!currentSlots[slotPosFrom].IsEmpty() && !currentSlots[slotPosTo].IsEmpty())
        {
            if (currentSlots[slotPosFrom].ID == currentSlots[slotPosTo].ID)
            {
                int maxStack = ItemManager.GetInstance().GetItemFromID(currentSlots[slotPosTo].ID).maxStack;
                if (maxStack > 1)
                {
                    currentSlots[slotPosFrom].amount = currentSlots[slotPosTo].AddAmount(currentSlots[slotPosFrom].amount);
                    if (currentSlots[slotPosFrom].amount <= 0)
                    {
                        currentSlots[slotPosFrom].EmptySlot();
                    }
                    return;
                }
            }
        }
        Slot temp = new Slot(currentSlots[slotPosFrom].ID, currentSlots[slotPosFrom].amount);
        currentSlots[slotPosFrom] = currentSlots[slotPosTo];
        currentSlots[slotPosTo] = temp;
    }
    public bool UseItem(int slotPos)
    {
        Item item = ItemManager.GetInstance().GetItemFromID(currentSlots[slotPos].ID);
        if (item.GetType() == typeof(Consumible))
        {
            character.AddInitialAttack(((Consumible)item).attackUpgrade);
            character.AddCurrentDefense(((Consumible)item).defenseUpgrade);
            character.AddInitialSpeed(((Consumible)item).speedUpgrade);
            character.AddCurrentEnergy(((Consumible)item).currentEnergyUpgrade);
            character.AddInitialEnergy(((Consumible)item).maxEnergyUpgrade);
            character.AddInitialArmor(((Consumible)item).maxArmorUpgrade);
            character.AddCurrentArmor(((Consumible)item).currentArmorUpgrade);
            currentSlots[slotPos].AddAmount(-1);
            if (currentSlots[slotPos].IsEmpty())
                return false;
        }
        return true;
    }
    public void Divide(int slotPos)
    {
        if (currentSlots[slotPos].amount > 1)
        {
            int dividedAmount = (currentSlots[slotPos].amount / 2);
            if (currentSlots[slotPos].amount % 2 != 0) dividedAmount++;
            if (AddNewItemInEmptySlot(currentSlots[slotPos].ID, dividedAmount))
            {
                currentSlots[slotPos].amount /= 2;
            }
        }
    }
    public void Sort()
    {
        currentSlots.Sort();
    } public bool CanItemBeAdded(int id, int amount)
    {
        int currentEmptySpaces = 0;
        for (int i = 0; i < currentSlots.Count; i++)
        {
            if (currentSlots[i].IsEmpty())
            {
                return true;
            }
            else if (id == currentSlots[i].ID)
            {
                currentEmptySpaces += ItemManager.GetInstance().GetItemFromID(id).maxStack - currentSlots[i].amount;
                if(currentEmptySpaces >= amount)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CheckForItem(Item item, int amount)
    {
        int itemID = item.id;
        int currentItemAmount = 0;
        foreach (var slot in currentSlots)
        {
            if(slot.ID == itemID)
            {
                currentItemAmount += slot.amount;
                if(currentItemAmount >= amount)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void RemoveItem(Item item, int amount)
    {
        int itemID = item.id;
        foreach (var slot in currentSlots)
        {
            if (slot.ID == itemID)
            {
                slot.amount -= amount;
                if(slot.amount > 0)
                {
                    return;
                }
                else
                {
                    amount = -slot.amount;
                    slot.EmptySlot();
                    if(amount == 0)
                    {
                        return;
                    }
                }
            }
        }
    }
    public void BlowUpInventory()
    {
        foreach (var slot in currentSlots)
        {
            if (slot.ID > 0)
            {
                Vector3 randomForceDirection = UnityEngine.Random.insideUnitSphere * explosionStrenght;
                Debug.DrawRay(transform.position, randomForceDirection * 10, Color.red, 10);
                Transform parent = ItemManager.GetInstance().transform;
                Vector3 posAux = transform.position;
                posAux.y += 1;
                GameObject go = Instantiate(ItemManager.GetInstance().GetItemFromID(slot.ID).worldPrefab, posAux, Quaternion.identity, parent);
                var itemComponent = go.GetComponent<ItemComponent>();
                itemComponent.AddForce(randomForceDirection);
                itemComponent.SetItem(slot.ID, slot.amount);
            }
        }
    }
    IEnumerator PickUpCoroutine()
    {
        while (canPickUpItems)
        {
            var colliders = Physics.OverlapSphere(transform.position, pickUpDistance, pickUpMask);

            foreach (var item in colliders)
            {
                ItemComponent itemComponent = item.GetComponent<ItemComponent>();
                if(AddNewItem(itemComponent.GetID(), itemComponent.GetAmount()))
                {
                    OnPickUp?.Invoke(ItemManager.GetInstance().GetItemFromID(itemComponent.GetID()), itemComponent.GetAmount());
                    Destroy(item.gameObject);
                }
            }
            yield return null;
        }
    }

    public int GetSize()
    {
        return size;
    }
    public Slot GetSlot(int index)
    {
        return currentSlots[index];
    }
    public void SetSlot(int index, Slot slot)
    {
        currentSlots[index] = slot;
    }
    public int GetID(int index)
    {
        return currentSlots[index].ID;
    }
    public List<Slot> GetInventoryList()
    {
        return currentSlots;
    }
}
