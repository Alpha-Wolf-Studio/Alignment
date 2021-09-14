using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiQuest : MonoBehaviour
{
    public QuestHandler questHandler;
    public UiTask pfUiTask;
    [SerializeField] private RectTransform panelQuest;
    [SerializeField] private TextMeshProUGUI nameQuest;
    [SerializeField] private RectTransform panelTask;
    private Quest quest;
    private List<UiTask> allUiTasks = new List<UiTask>();

    void Start()
    {
        SetQuest();
        questHandler.OnTaskProgress += RefreshAllTask;  // todo: nunca se llama a este evento cuando pickeo o crafteo los items necesarios del task.
    }
    void SetQuest()
    {
        ClearTasks();
        quest = questHandler.allQuest[questHandler.GetCurrentQuest()];
        int maxTasks = quest.tasks.Count;

        for (int i = 0; i < maxTasks; i++)
        {
            UiTask uiTask = Instantiate(pfUiTask, panelTask);
            allUiTasks.Add(uiTask);
            uiTask.toggle.isOn = false;
            SetTask(ref uiTask, quest.tasks[i], i);
        }

        nameQuest.text = questHandler.allQuest[questHandler.GetCurrentQuest()].questTitle;
    }
    void SetTask(ref UiTask uiTask,Task task, int i)
    {
        string type = task.GetType().ToString();
        switch (type)
        {
            case "GrabItemTask":
                GrabItemTask grab = ((GrabItemTask)task);
                uiTask.name.text = "Collect " + grab.amount + " of " + grab.itemToGrab.itemName;
                uiTask.description.text = "kill dinosaur to get this item";
                break;
            case "CraftItemTask":
                CraftItemTask craft = ((CraftItemTask)task);
                uiTask.name.text = "Craft " + craft.amount + " of " + craft.itemToCraft.itemName;
                uiTask.description.text = "Open inventory with E and craft " + craft.itemToCraft.itemName;
                break;
            case "KillDinosTask":
                KillDinosTask kills = ((KillDinosTask)task);
                uiTask.name.text = "Do not have Name yet";
                uiTask.description.text = "Do not have Description yet";
                break;
            case "RepairTask":
                RepairTask repair = ((RepairTask)task);
                uiTask.name.text = "Do not have Name yet";
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
        for (int i = 0; i < quest.tasks.Count; i++)
        {
            bool finished = quest.tasks[i].IsFinished();
            allUiTasks[i].toggle.isOn = finished;
            Debug.Log("Task: " + allUiTasks[i].name + " Terminada: " + finished);
        }
    }
}