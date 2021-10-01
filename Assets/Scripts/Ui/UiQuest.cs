using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiQuest : MonoBehaviour
{
    public UiTask pfUiTask;
    [SerializeField] private RectTransform panelQuest;
    [SerializeField] private TextMeshProUGUI nameQuest;
    [SerializeField] private RectTransform panelTask;
    private QuestHandler questHandler;
    //private Quest quest;
    private List<UiTask> allUiTasks = new List<UiTask>();
    private List<SubQuest> tasks;

    private void Awake()
    {
        questHandler = GameManager.Get().questHandler;
    }
    void Start()
    {
        questHandler.OnQuestCompleted += SetQuest;
        SetQuest();
        questHandler.OnTaskProgress += RefreshAllTask;
    }
    void SetQuest()
    {
        ClearTasks();
        tasks = questHandler.GetAllTask();

        for (int i = 0; i < tasks.Count; i++)
        {
            UiTask uiTask = Instantiate(pfUiTask, panelTask);
            allUiTasks.Add(uiTask);
            uiTask.toggle.isOn = false;
            SetTask(ref uiTask, tasks[i], i);
        }

        nameQuest.text = "(Q) " + questHandler.allQuest[questHandler.GetCurrentQuest()].questTitle;
    }
    void SetTask(ref UiTask uiTask,SubQuest task, int i)
    {

        switch (task.type)
        {
            case SubQuest.SubQuestType.KILL:
                uiTask.nameTask.text = "Do not have Name yet";
                uiTask.description.text = "Do not have Description yet";
                break;
            case SubQuest.SubQuestType.PICKUP:
                uiTask.nameTask.text = "Collect " + task.pickUpAmount + " of " + task.itemToPickUp.itemName;
                uiTask.description.text = "kill dinosaurs to get this item";
                break;
            case SubQuest.SubQuestType.CRAFT:
                uiTask.nameTask.text = "Craft " + task.craftAmount + " of " + task.itemToCraft.itemName;
                uiTask.description.text = "Open inventory with " + "E" + " and craft " + task.itemToCraft.itemName;
                break;
            case SubQuest.SubQuestType.REPAIR:
                uiTask.nameTask.text = "Do not have Name yet";
                uiTask.description.text = "Do not have Description yet";
                break;
            default:
                Debug.LogError("El index " + i + " no está en el switch de la quest: ", gameObject);
                break;
        }
    }
    void ClearTasks()
    {
        allUiTasks.Clear();
        UiTask[] allTasks = FindObjectsOfType<UiTask>();
        for (int i = 0; i < allTasks.Length; i++)
        {
            Destroy(allTasks[i].gameObject);
        }
    }
    void RefreshAllTask()
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            bool finished = tasks[i].IsCompleted();
            allUiTasks[i].toggle.isOn = finished;
            Debug.Log("Task: " + allUiTasks[i].name + " Terminada: " + finished);
        }
    }
}