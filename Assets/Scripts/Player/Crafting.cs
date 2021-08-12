using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Crafting : MonoBehaviour
{
    Inventory inventory = null;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    public bool Craft(Item item)
    {
        int itemsAmount = 0;
        foreach (var ingredient in item.recipe)
        {
            if (inventory.CheckForItem(ingredient.item, ingredient.amount))
            {
                itemsAmount++;
            }
        }
        if(itemsAmount == item.recipe.Count)
        {
            foreach (var ingredient in item.recipe)
            {
                inventory.RemoveItem(ingredient.item, ingredient.amount);
            }
            inventory.AddNewItem(item.id, 1);
            return true;
        }
        return false;
    }
}
