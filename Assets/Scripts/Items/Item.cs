using UnityEngine;

[CreateAssetMenu(fileName = "Consumible", menuName = "Items/Consumible")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public Mesh mesh;
    public Material material;
    public int maxStack = 1;
    public bool consumible = false;
    public bool crafteable = true;

    public virtual string ItemToString() { return "Name: " + itemName; }
}