﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{

    public Action OnQuestCompleted;
    public Action OnTaskProgress;

    public List<Quest> allQuest = new List<Quest>();
    [SerializeField] List<SubQuest> currentTasks = new List<SubQuest>();
    int currentQuest = -1;
    public int GetCurrentQuest() => currentQuest;
    public List<SubQuest> GetAllTask() => currentTasks;

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
        StartNewQuest();
    }

    private void Start()
    {

    }

    void StartNewQuest()
    {
        currentQuest++;
        currentTasks.Clear();
        for (int i = 0; i < allQuest[currentQuest].tasks.Count; i++)
        {
            int killAmount = allQuest[currentQuest].tasks[i].killAmount;
            int pickUpAmount = allQuest[currentQuest].tasks[i].pickUpAmount;
            int craftAmount = allQuest[currentQuest].tasks[i].craftAmount;
            SubQuest t = new SubQuest(allQuest[currentQuest].tasks[i], killAmount, pickUpAmount, craftAmount);
            currentTasks.Add(t);
        }
    }

    public List<SubQuest> GetCurrentTasks()
    {
        return currentTasks;
    }

    private void RepairEvent(RepairLocations location)
    {
        foreach (var task in currentTasks)
        {
            if (!task.IsCompleted() && RepairCheck(task, location))
            {
                task.Complete();
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool RepairCheck(SubQuest task, RepairLocations location)
    {
        return task.type == SubQuest.SubQuestType.REPAIR &&
               task.locationToRepair == location;
    }

    private void PickUpEvent(Item item, int amount)
    {
        for (int index = 0; index < currentTasks.Count; index++)
        {
            //var task = currentTasks[index];
            if (!currentTasks[index].IsCompleted() && PickUpCheck(currentTasks[index], item))
            {
                currentTasks[index].pickUpAmount -= amount;
                if (currentTasks[index].pickUpAmount <= 0)
                {
                    currentTasks[index].completed = true;
                    //task.Complete();    // todo: no se está moficicando a true la TASK.
                }

                OnTaskProgress?.Invoke();
            }
        }
    }

    bool PickUpCheck(SubQuest task, Item item)
    {
        bool taskType = task.type == SubQuest.SubQuestType.PICKUP;
        if (!taskType) return false;
        bool itemType = task.itemToPickUp == item;
        return itemType;
    }

    private void CraftEvent(Item item)
    {
        foreach (var task in currentTasks)
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
    }

    bool CraftCheck(SubQuest task, Item item)
    {
        return task.type == SubQuest.SubQuestType.CRAFT &&
               task.itemToCraft == item;
    }

    void DinoDiedEvent(DinoClass dino)
    {
        foreach (var task in currentTasks)
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
    }

    bool DinoCheck(SubQuest task, DinoClass dino)
    {
        return task.type == SubQuest.SubQuestType.KILL &&
               task.dinosaursToKill == dino;
    }

}
