using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataCmd
{
    public string name;
    public Console.Method cmd;
    public string description;
    public override string ToString() => " -> " + description;
}
public class Console : MonoBehaviour, IPointerClickHandler
{
    public delegate void Method();

    private PlayerController player;
    private Inventory invPlayer;
    private Character character;
    public TextMeshProUGUI textConsole;
    private RectTransform rtConsole;
    public TMP_InputField inputField;

    public Dictionary<string, DataCmd> consoleCommands = new Dictionary<string, DataCmd>();
    private PlayerController.PlayerStatus lastPlayerStatus;
    private Vector2 startConsoleSize;

    private bool pause;
    private bool hud = true;
    public GameObject cvHud;
    public ModuleCheat cheats;

    private void Awake()
    {
        player = GameManager.Get().player;
        invPlayer = player.GetComponent<Inventory>();
        character = GameManager.Get().character;
        rtConsole = gameObject.GetComponent<RectTransform>();
    }private void Start()
    {
        startConsoleSize = rtConsole.sizeDelta;
        AllCmd();
        Clear();
        inputField.text = "";
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        lastPlayerStatus = player.playerStatus;
        player.playerStatus = PlayerController.PlayerStatus.Console;
        inputField.Select();
        player.AvailableCursor(true);
    }
    private void OnDisable()
    {
        player.playerStatus = lastPlayerStatus;
        player.AvailableCursor(false);
    }
    void Update()
    {
        inputField.Select();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
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
        }
    }
    void AllCmd()   // Se cargan todas las funciones de la consola
    {
        AddCommand("clear", Clear, "Clean the Console.");
        AddCommand("help", Help, "Show help.");
        AddCommand("expand", ExpandConsole, "Expand the Console.");
        AddCommand("contract", ContractConsole, "Retract the Console.");
        AddCommand("pause", PauseGame, "Alternate game pause.");
        AddCommand("hud", ToggleHud, "Disable HUD.");

        AddCommand("cheat armor", InfinityArmor, "Infinity Armor.");
        AddCommand("cheat energy", InfinityEnergy, "Infinity Energy.");
        AddCommand("cheat stamina", InfinityStamina, "Infinity Stamina.");
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
        rtConsole.sizeDelta = startConsoleSize;
    }
    void PauseGame()
    {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        inputField.Select();
    }
    public void ToggleHud()
    {
        hud = !hud;
        cvHud.SetActive(hud);
    }
    void InfinityArmor()
    {
        cheats.CheatEnable(Character.Stats.Armor);
    }
    void InfinityEnergy()
    {
        cheats.CheatEnable(Character.Stats.Energy);
    }
    void InfinityStamina()
    {
        //cheats.CheatEnable(Character.Stats);
    }
}