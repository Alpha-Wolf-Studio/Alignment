using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DataCmd
{
    public string name;
    public Console.Method cmd;
    public string description;
    public override string ToString() => " -> " + description;
}
public class Console : MonoBehaviour
{
    public delegate void Method();

    public PlayerController player;
    private Inventory invPlayer;
    private Character character;
    public TextMeshProUGUI textConsole;
    private RectTransform rtConsole;
    public TMP_InputField inputField;

    public Dictionary<string, DataCmd> consoleCommands = new Dictionary<string, DataCmd>();

    private void Awake()
    {
        rtConsole = gameObject.GetComponent<RectTransform>();
    }
    private void Start()
    {
        AllCmd();
        Clear();
        inputField.text = "";
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        inputField.Select();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string text = inputField.text.ToLower();
            Write(text);
            if (consoleCommands.ContainsKey(text))
            {
                try
                {
                    consoleCommands[text].cmd.Invoke();
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                    textConsole.text += "\n" + "----------ERROR----------";
                    textConsole.text += "\n" + e.Message + "\n" + e.StackTrace;
                }
            }
            else
            {
                textConsole.text += "  <---- Command don't exist.";
            }
            inputField.Select();
        }
    }
    void AllCmd()   // Se cargan todas las funciones de la consola
    {
        AddCommand("clear", Clear, "Clean the Console.");
        AddCommand("help", Help, "Show help.");
        AddCommand("expand", ExpandConsole, "Expand the Console.");
        AddCommand("contract", ContractConsole, "Retract the Console.");
        AddCommand("pause on", PauseGame, "Force game pause.");
        AddCommand("pause off", UnPauseGame, "Force game unpause.");
    }
    void AddCommand(string cmdName, Method cmdCommand, string cmdDescription)
    {
        DataCmd cmd = new DataCmd
        {
            name = cmdName,
            cmd = cmdCommand,
            description = cmdDescription
        };

        consoleCommands.Add(cmdName, cmd);
    }
    void Write(string text)
    {
        textConsole.text += "\n" + text;
        inputField.text = "";
    }
    void Clear()
    {
        textConsole.text = "";
    }
    void Help()
    {
        foreach (var cmd in consoleCommands)
        {
            Write(cmd.ToString());
        }
    }
    void ExpandConsole()
    {
        rtConsole.sizeDelta = new Vector2(rtConsole.sizeDelta.x, 700);
    }
    void ContractConsole()
    {
        rtConsole.sizeDelta = new Vector2(rtConsole.sizeDelta.x, 235);
    }
    void PauseGame()
    {
        Time.timeScale = 0;
    }
    void UnPauseGame()
    {
        Time.timeScale = 0;
    }
}