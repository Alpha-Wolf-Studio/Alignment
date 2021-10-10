﻿using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Crafting : MonoBehaviour
{

    public Action<Item> OnCraft;

    Inventory inventory = null;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    public void Craft(Item item)
    {
        if(IsCraftPosible(item))
        {
            foreach (var ingredient in item.recipe)
            {
                inventory.RemoveItem(ingredient.item, ingredient.amount);
            }
            inventory.AddNewItem(item.id, 1);
            OnCraft?.Invoke(item);
            if (Sfx.Get().GetEnable(Sfx.ListSfx.UiCraftSuccessful))
                AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiCraftSuccessful), gameObject);
        }
        else
        {
            if (Sfx.Get().GetEnable(Sfx.ListSfx.UiCraftFail))
                AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiCraftFail), gameObject);
        }
    }
    public bool IsCraftPosible(Item item)
    {
        int itemsAmount = 0;
        if (item.recipe.Count > 0) 
        {
            foreach (var ingredient in item.recipe)
            {
                if (ingredient.item)
                {
                    if (inventory.CheckForItem(ingredient.item, ingredient.amount))
                    {
                        itemsAmount++;
                    }
                }
                else
                {
                    Debug.LogWarning("Se jodió: " + item.itemName);
                }
            }
        }
        else
        {
            Debug.LogWarning("No se cargó la lista.");
        }
        return itemsAmount == item.recipe.Count;
    }

    public int PosibleCraftAmount(Item item) 
    {
        int posibleCrafts = 0;
        bool nextCraftPosible;

        do
        {
            nextCraftPosible = false;
            int itemsAmount = 0;
            if (item.recipe.Count > 0)
            {
                foreach (var ingredient in item.recipe)
                {
                    if (ingredient.item)
                    {
                        if (inventory.CheckForItem(ingredient.item, ingredient.amount * posibleCrafts))
                        {
                            itemsAmount++;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Se jodió: " + item.itemName);
                    }
                }
                if(itemsAmount == item.recipe.Count) 
                {
                    nextCraftPosible = true;
                    posibleCrafts++;
                }
            }
        } while (nextCraftPosible);

        return posibleCrafts;
    }

}