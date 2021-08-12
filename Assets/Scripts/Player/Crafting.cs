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
        List<RecipeIngredient> ingridients = new List<RecipeIngredient>();
        inventory.AddNewItem(item.id, 1);
        return true;
    }



}
