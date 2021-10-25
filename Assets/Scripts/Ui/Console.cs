using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;

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
    private Entity character;
    public RectTransform content;
    public TextMeshProUGUI textConsole;
    private RectTransform rtConsole;
    public TMP_InputField inputField;

    public Dictionary<string, DataCmd> consoleCommands = new Dictionary<string, DataCmd>();
    private PlayerController.PlayerStatus lastPlayerStatus;
    private Vector2 startConsoleSize;

    private bool pause;
    private bool hud = true;
    public GameObject cvHud;
    private ModuleCheat cheats;
    private float expandOnWrite = 43.5f;
    private void Awake()
    {
        cheats = GetComponent<ModuleCheat>();
        player = GameManager.Get().player;
        invPlayer = player.GetComponent<Inventory>();
        character = GameManager.Get().playerEntity;
        rtConsole = gameObject.GetComponent<RectTransform>();
    }
    private void Start()
    {
        startConsoleSize = rtConsole.sizeDelta;
        AllCmd();
        Clear();
        inputField.text = "";
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        PauseGame(true);
        lastPlayerStatus = player.GetStatus();
        player.ChangeStatus(PlayerController.PlayerStatus.Console);
        inputField.Select();
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        player.AvailableCursor(true);
    }
    private void OnDisable()
    {
        PauseGame(false);
        player.ChangeStatus(lastPlayerStatus);
        player.AvailableCursor(false);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
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
                textConsole.text += "  <---- Command don't exist. Type: help";
            }
            EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }
    private void AllCmd()   // Se cargan todas las funciones de la consola
    {
        AddCommand("clear", Clear, "Clean the Console.");
        AddCommand("help", Help, "Show help.");
        AddCommand("expand", ExpandConsole, "Expand the Console.");
        AddCommand("contract", ContractConsole, "Retract the Console.");
        AddCommand("pause", PauseGame, "Alternate game pause.");
        AddCommand("hud", ToggleHud, "Disable HUD.");
        AddCommand("kill enemy", KillEnemies, "kill all enemies.");
        AddCommand("inv clear", ClearInventory, "Clear your Inventory.");
        AddCommand("inv add", AddFiveSlotsInventory, "Add 5 slots in Inventory.");

        AddCommand("cheat armor", InfinityArmor, "Infinity Armor.");
        AddCommand("cheat energy", InfinityEnergy, "Infinity Energy.");
        AddCommand("cheat stamina", InfinityStamina, "Infinity Stamina.");
        AddCommand("cheat jetpack", AddJetPack, "Add Jetpack.");
        AddCommand("cheat godmode", AddGodMode, "Add GodMode.");
    }
    private void AddCommand(string cmdName, Method cmdCommand, string cmdDescription)
    {
        DataCmd cmd = new DataCmd
        {
            name = cmdName,
            cmd = cmdCommand,
            description = cmdDescription
        };

        consoleCommands.Add(cmdName, cmd);
    }
    private void Write(string text)
    {
        textConsole.text += "\n" + text;
        inputField.text = "";
        ExpandContent(false);
    }
    private void Clear()
    {
        textConsole.text = "";
        ExpandContent(true);
    }
    private void Help()
    {
        foreach (var cmd in consoleCommands)
        {
            Write(cmd.ToString());
        }
    }
    private void ExpandConsole()
    {
        rtConsole.sizeDelta = new Vector2(rtConsole.sizeDelta.x, 700);
    }
    private void ContractConsole()
    {
        rtConsole.sizeDelta = startConsoleSize;
    }
    private void PauseGame()
    {
        pause = !pause;
        Time.timeScale = pause ? 0 : 1;
    }
    private void PauseGame(bool on)
    {
        pause = on;
        Time.timeScale = on ? 0 : 1;
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
    private void InfinityArmor()
    {
        cheats.CheatEnable(character.entityStats.GetStat(StatType.Armor));
    }
    private void InfinityEnergy()
    {
        cheats.CheatEnable(character.entityStats.GetStat(StatType.Energy));
    }
    private void InfinityStamina()
    {
        cheats.CheatEnable(character.entityStats.GetStat(StatType.Stamina));
    }
    private void AddJetPack()
    {
        player.jetpack = !player.jetpack;
    }
    void ClearInventory()
    {
        invPlayer.ClearInventory();
    }
    private void AddFiveSlotsInventory()
    {
        invPlayer.AddSlots(5);
    }
    private void AddGodMode()
    {
        InfinityArmor();
        InfinityEnergy();
        InfinityStamina();
        AddJetPack();
    }
    private void KillEnemies()
    {
        Entity[] characters = FindObjectsOfType<Entity>();

        for (int i = 0; i < characters.Length; i++)
        {
            PlayerController player = characters[i].GetComponent<PlayerController>();
            if (!player)
            {
                DamageInfo info = new DamageInfo(9999, DamageOrigin.Water, DamageType.Armor);
                characters[i].TakeDamage(info);
            }
        }
    }
    public void ExpandContent(bool resetSize)
    {
        Vector2 size = content.sizeDelta;
        size.y = resetSize ? 0 : size.y + expandOnWrite;
        content.sizeDelta = size;
    }
}