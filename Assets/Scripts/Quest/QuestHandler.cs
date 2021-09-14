using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{

    public Action OnQuestCompleted;
    public Action OnTaskProgress;

    public List<Quest> allQuest = new List<Quest>();
    [SerializeField] List<Task> currentTasks = new List<Task>();
    int currentQuest = -1;
    public int GetCurrentQuest() => currentQuest;

    [Header("Repair Locations References")]
    [SerializeField] ReparableObject machine = null;
    [SerializeField] ReparableObject oxigen = null;
    [SerializeField] ReparableObject generator = null;

    [Header("Player references")]
    [SerializeField] Inventory inventory = null;
    [SerializeField] Crafting crafting = null;

    [Header("Enemy References")]
    [SerializeField] EnemyManager enemyManager = null;

    private void Awake()
    {
        machine.OnRepair += RepairEvent;
        oxigen.OnRepair += RepairEvent;
        generator.OnRepair += RepairEvent;
        inventory.OnPickUp += PickUpEvent;
        crafting.OnCraft += CraftEvent;
        enemyManager.OnDinoDied += DinoDiedEvent;
    }

    private void Start()
    {
        StartNewQuest();
    }

    void StartNewQuest()
    {
        currentQuest++;
        currentTasks.Clear();
        var quest = Instantiate(allQuest[currentQuest]);
        allQuest[currentQuest] = quest;
        for (int i = 0; i < quest.tasks.Count; i++)
        {
            currentTasks.Add(quest.tasks[i]);
        }
    }

    public List<Task> GetCurrentTasks()
    {
        return currentTasks;
    }

    private void RepairEvent(RepairLocations location)
    {
        foreach (var task in allQuest[currentQuest].tasks)
        {
            if (!task.isCompleted() && RepairCheck(task, location))
            {
                task.complete();
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool RepairCheck(Task task, RepairLocations location)
    {
        return task.type == Task.TaskType.REPAIR &&
               task.locationToRepair == location;
    }

    private void PickUpEvent(Item item, int amount)
    {
        foreach (var task in allQuest[currentQuest].tasks)
        {
            if (!!task.isCompleted() && PickUpCheck(task, item))
            {
                task.pickUpAmount -= amount;
                if (task.pickUpAmount <= 0)
                {
                    task.complete();
                }
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool PickUpCheck(Task task, Item item)
    {
        bool taskType = task.type == Task.TaskType.PICKUP;
        if (!taskType) return false;
        bool itemType = task.itemToPickUp == item;
        return itemType;
    }

    private void CraftEvent(Item item)
    {
        foreach (var task in allQuest[currentQuest].tasks)
        {
            if (!!task.isCompleted() && CraftCheck(task, item))
            {
                task.craftAmount--;
                if (task.craftAmount <= 0)
                {
                    task.complete();
                }
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool CraftCheck(Task task, Item item)
    {
        return task.type == Task.TaskType.CRAFT &&
               task.itemToCraft == item;
    }

    void DinoDiedEvent(DinoClass dino)
    {
        foreach (var task in allQuest[currentQuest].tasks)
        {
            if (!!task.isCompleted() && DinoCheck(task, dino))
            {
                task.killAmount--;
                if (task.killAmount <= 0)
                {
                    task.complete();
                }
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool DinoCheck(Task task, DinoClass dino)
    {
        return task.type == Task.TaskType.KILL &&
               task.dinosaursToKill == dino;
    }

}
