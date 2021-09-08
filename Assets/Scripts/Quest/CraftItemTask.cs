using UnityEngine;

[CreateAssetMenu(fileName = "Craft Item Task", menuName = "Quest/Task/Craft Item")]
public class CraftItemTask : Task
{
    public Item itemToCraft = null;
    public int amount = 1;
}
