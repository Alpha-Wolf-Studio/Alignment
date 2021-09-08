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
            currentTasks.Add(Instantiate(quest.tasks[i]));
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
            if(!task.IsFinished() && RepairCheck(task, location)) 
            {
                task.Finish();
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool RepairCheck(Task task, RepairLocations location) 
    {
        return task.GetType() == typeof(RepairTask) &&
               ((RepairTask)task).locationToRepair == location;
    }

    private void PickUpEvent(Item item, int amount)
    {
        foreach (var task in allQuest[currentQuest].tasks)
        {
            if (!task.IsFinished() && PickUpCheck(task, item))
            {
                ((GrabItemTask)task).amount -= amount;
                if(((GrabItemTask)task).amount <= 0) 
                {
                    task.Finish();
                }
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool PickUpCheck(Task task, Item item) 
    {
        return task.GetType() == typeof(GrabItemTask) &&
               ((GrabItemTask)task).itemToGrab == item;
    }

    private void CraftEvent(Item item)
    {
        foreach (var task in allQuest[currentQuest].tasks)
        {
            if (!task.IsFinished() && CraftCheck(task, item))
            {
                ((CraftItemTask)task).amount--;
                if (((CraftItemTask)task).amount <= 0)
                {
                    task.Finish();
                }
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool CraftCheck(Task task, Item item) 
    {
        return task.GetType() == typeof(CraftItemTask) &&
               ((CraftItemTask)task).itemToCraft == item;
    }

    void DinoDiedEvent(DinoClass dino) 
    {
        foreach (var task in allQuest[currentQuest].tasks)
        {
            if (!task.IsFinished() && DinoCheck(task, dino))
            {
                ((KillDinosTask)task).amount--;
                if (((KillDinosTask)task).amount <= 0)
                {
                    task.Finish();
                }
                OnTaskProgress?.Invoke();
            }
        }
    }

    bool DinoCheck(Task task, DinoClass dino) 
    {
        return task.GetType() == typeof(KillDinosTask) &&
               ((KillDinosTask)task).dinosaurToKill == dino;
    }

}
