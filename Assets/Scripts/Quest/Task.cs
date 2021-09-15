using UnityEngine;

[System.Serializable]
public class Task
{
    public enum TaskType { KILL, PICKUP, CRAFT, REPAIR }

    public TaskType type;
    bool completed = false;
    public void complete() => completed = true;
    public bool isCompleted() => completed;

    public DinoClass dinosaursToKill = DinoClass.Raptor;
    public int killAmount = 1;

    public Item itemToPickUp = null;
    public int pickUpAmount = 1;

    public Item itemToCraft = null;
    public int craftAmount = 1;

    public RepairLocations locationToRepair;
}