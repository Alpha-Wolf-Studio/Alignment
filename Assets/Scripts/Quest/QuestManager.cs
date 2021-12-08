using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviourSingleton<QuestManager>
{

    public Action OnQuestCompleted;
    public Action OnTaskProgress;
    public Action OnRepairedShip;

    public List<Quest> allQuest = new List<Quest>();
    [SerializeField] List<SubQuest> currentSubquest = new List<SubQuest>();
    int currentQuest = -1;
    public int GetCurrentQuest() => currentQuest;
    public List<SubQuest> GetAllTask() => currentSubquest;

    [Header("Repair Locations References")]
    ReparableObject[] objectsToRepair;
    List<RepairLocations> repairedLocations;

    [Header("Player references")]
    [SerializeField] Inventory inventory = null;
    [SerializeField] Crafting crafting = null;

    [Header("Enemy References")]
    [SerializeField] EnemyManager enemyManager = null;

    [HideInInspector] public bool questsDone;

    private void Start()
    {
        objectsToRepair = FindObjectsOfType<ReparableObject>();
        foreach (var repairObjects in objectsToRepair)
        {
            repairObjects.OnRepair += RepairEvent;
        }

        inventory.OnPickUp += PickUpEvent;
        crafting.OnCraft += CraftEvent;
        enemyManager.OnDinoDied += DinoDiedEvent;
        StartNewQuest();
    }
    void StartNewQuest()
    {
        if (!questsDone)
        {
            currentQuest++;
            currentSubquest.Clear();

            if (currentQuest < allQuest.Count)
            {
                for (int i = 0; i < allQuest[currentQuest].tasks.Count; i++)
                {
                    int killAmount = allQuest[currentQuest].tasks[i].killAmount;
                    int pickUpAmount = allQuest[currentQuest].tasks[i].pickUpAmount;
                    int craftAmount = allQuest[currentQuest].tasks[i].craftAmount;
                    SubQuest t = new SubQuest(allQuest[currentQuest].tasks[i], killAmount, pickUpAmount, craftAmount);
                    currentSubquest.Add(t);
                    if(t.type == SubQuest.SubQuestType.REPAIR && repairedLocations.Contains(t.locationToRepair)) 
                    {
                        t.Complete();
                        OnTaskProgress?.Invoke();
                        AkSoundEngine.PostEvent(AK.EVENTS.COMPLETEDTASK, gameObject);
                    }
                }
            }
            else
            {
                questsDone = true;
                Debug.Log("No hay mas Quests. Cant quests: " + allQuest.Count, gameObject);
            }
        }
    }
    public List<SubQuest> GetCurrentTasks()
    {
        return currentSubquest;
    }
    private void RepairEvent(RepairLocations location)
    {
        foreach (var task in currentSubquest)
        {
            if (!task.IsCompleted() && RepairCheck(task, location))
            {
                task.Complete();
                OnTaskProgress?.Invoke();
                AkSoundEngine.PostEvent(AK.EVENTS.COMPLETEDTASK, gameObject);
            }
        }

        if (IsCurrentQuestDone())
        {
            StartNewQuest();
            OnQuestCompleted?.Invoke();
            AkSoundEngine.PostEvent(AK.EVENTS.COMPLETEDQUEST, gameObject);
        }

        repairedLocations.Add(location);

        if (repairedLocations.Count == objectsToRepair.Length) 
        {
            OnRepairedShip?.Invoke();
        }

    }
    bool RepairCheck(SubQuest task, RepairLocations location)
    {
        return task.type == SubQuest.SubQuestType.REPAIR &&
               task.locationToRepair == location;
    }
    public void PickUpEvent(Item item, int amount)
    {
        foreach (var task in currentSubquest)
        {
            //var task = currentTasks[index];
            if (!task.IsCompleted() && PickUpCheck(task, item))
            {
                task.pickUpAmount -= amount;
                if (task.pickUpAmount <= 0)
                {
                    task.Complete();
                }
                OnTaskProgress?.Invoke();
            }
        }
        if (IsCurrentQuestDone())
        {
            StartNewQuest();
            OnQuestCompleted?.Invoke();
        }
    }
    bool PickUpCheck(SubQuest task, Item item)
    {
        bool taskType = task.type == SubQuest.SubQuestType.PICKUP;
        if (!taskType) return false;
        bool itemType = task.itemToPickUp == item;
        return itemType;
    }
    public void CraftEvent(Item item)
    {
        foreach (var task in currentSubquest)
        {
            if (!task.IsCompleted() && CraftCheck(task, item))
            {
                task.craftAmount--;
                if (task.craftAmount <= 0)
                {
                    task.Complete();
                }
                OnTaskProgress?.Invoke();
            }
        }
        if (IsCurrentQuestDone())
        {
            StartNewQuest();
            OnQuestCompleted?.Invoke();
        }
    }
    bool CraftCheck(SubQuest task, Item item)
    {
        return task.type == SubQuest.SubQuestType.CRAFT &&
               task.itemToCraft == item;
    }
    public void DinoDiedEvent(DinoType dino)
    {
        foreach (var task in currentSubquest)
        {
            if (!task.IsCompleted() && DinoCheck(task, dino))
            {
                task.killAmount--;
                if (task.killAmount <= 0)
                {
                    task.Complete();
                }
                OnTaskProgress?.Invoke();
            }
        }
        if (IsCurrentQuestDone())
        {
            StartNewQuest();
            OnQuestCompleted?.Invoke();
        }
    }
    bool DinoCheck(SubQuest task, DinoType dino)
    {
        return task.type == SubQuest.SubQuestType.KILL &&
               task.dinosaursToKill == dino;
    }
    bool IsCurrentQuestDone()
    {
        if (questsDone) 
            return false;

        foreach (var task in currentSubquest)
        {
            if (!task.IsCompleted())
            {
                return false;
            }
        }
        return true;
    }
}
