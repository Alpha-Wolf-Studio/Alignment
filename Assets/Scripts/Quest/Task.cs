using UnityEngine;

[System.Serializable]
public class Task
{
    public enum TaskType { KILL, PICKUP, CRAFT, REPAIR }

    [Header("General")]
    public TaskType type;
    bool completed = false;
    public void complete() => completed = true;
    public bool isCompleted() => completed;

    [Header("Kill")]
    public DinoClass dinosaursToKill = DinoClass.Raptor;
    public int killAmount = 1;

    [Header("Pick Up")]
    public Item itemToPickUp = null;
    public int pickUpAmount = 1;

    [Header("Craft")]
    public Item itemToCraft = null;
    public int craftAmount = 1;

    [Header("Repair")]
    public RepairLocations locationToRepair;
}