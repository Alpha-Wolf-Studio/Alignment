using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "General", menuName = "Items/General")]
public class Item : ScriptableObject
{
    [Header("Item General")]
    public string itemName;
    public int id;
    public Sprite icon;
    public GameObject worldPrefab;
    public int maxStack = 1;
    public bool crafteable = true;

    public List<RecipeIngredient> recipe = null;

    public virtual string ItemToString() { return "Name: " + itemName; }
}

[System.Serializable]
public class RecipeIngredient
{
    public Item item = null;
    public int amount = 1;
}