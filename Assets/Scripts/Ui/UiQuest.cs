using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiQuest : MonoBehaviour
{
    [SerializeField] private Sprite lastSpritesTasks;
    [SerializeField] private UiTask pfUiTask;
    [SerializeField] private TextMeshProUGUI nameQuest;
    [SerializeField] private RectTransform panelTask;

    private QuestManager questHandler;
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
        if (!QuestManager.Get().questsDone)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                UiTask uiTask = Instantiate(pfUiTask, panelTask);
                if (i == tasks.Count - 1)
                    uiTask.GetComponent<Image>().sprite = lastSpritesTasks;
                allUiTasks.Add(uiTask);
                uiTask.toggle.isOn = false;
                SetTask(ref uiTask, tasks[i], i);
            }

            nameQuest.text = "(Q) " + questHandler.allQuest[questHandler.GetCurrentQuest()].questTitle;
        }
        else
            nameQuest.text = "";
    }
    void SetTask(ref UiTask uiTask,SubQuest task, int i)
    {
        switch (task.type)
        {
            case SubQuest.SubQuestType.KILL:
                uiTask.nameTask.text = task.customDescription;
                break;
            case SubQuest.SubQuestType.PICKUP:
                uiTask.nameTask.text = task.customDescription;
                break;
            case SubQuest.SubQuestType.CRAFT:
                uiTask.nameTask.text = task.customDescription;
                break;
            case SubQuest.SubQuestType.REPAIR:
                uiTask.nameTask.text = task.customDescription;
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
            //Debug.Log("Task: " + allUiTasks[i].name + " Terminada: " + finished);
        }
    }
}