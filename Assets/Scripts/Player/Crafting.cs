using System;
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

    public bool Craft(Item item)
    {
        if(IsCraftPosible(item))
        {
            foreach (var ingredient in item.recipe)
            {
                inventory.RemoveItem(ingredient.item, ingredient.amount);
            }
            inventory.AddNewItem(item.id, 1);
            OnCraft?.Invoke(item);
            return true;
        }
        return false;
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
}