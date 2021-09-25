using UnityEngine;

[System.Serializable]
public class SubQuest
{

    public SubQuest(SubQuest t, int killAmount, int pickUpAmount, int craftAmount) 
    {
        type = t.type;
        dinosaursToKill = t.dinosaursToKill;
        this.killAmount = killAmount;

        itemToPickUp = t.itemToPickUp;
        this.pickUpAmount = pickUpAmount;

        itemToCraft = t.itemToCraft;
        this.craftAmount = craftAmount;
    }

    public enum SubQuestType { KILL, PICKUP, CRAFT, REPAIR }

    public SubQuestType type;
    public bool completed;

    public void Complete()
    {
        completed = true;
    }

    public bool IsCompleted()
    {
        return completed;
    }

    public DinoClass dinosaursToKill = DinoClass.Raptor;
    public int killAmount = 1;

    public Item itemToPickUp = null;
    public int pickUpAmount = 1;

    public Item itemToCraft = null;
    public int craftAmount = 1;

    public RepairLocations locationToRepair;
}