using UnityEngine;

[CreateAssetMenu(fileName = "Grab Item Task", menuName = "Quest/Task/Grab Item")]
public class GrabItemTask : Task
{
    public Item itemToGrab = null;
    public int amount = 1;
}
