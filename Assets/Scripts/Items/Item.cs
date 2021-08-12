using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumible", menuName = "Items/Consumible")]
public class Item : ScriptableObject
{
    public string itemName;
    public int id;
    public Sprite icon;
    public Mesh mesh;
    public Material material;
    public int maxStack = 1;
    public bool consumible = false;
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