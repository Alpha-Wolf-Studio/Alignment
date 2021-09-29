using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager gameManager;
    public Character character;
    public QuestHandler questHandler;
    public PlayerController player;
    public List<ReparableObject> toRepair = new List<ReparableObject>();
    private int objectsRemaining;

    public static GameManager Get() => gameManager;
    private void Awake()
    {
        gameManager = this;
    } void Start()
    {
        LoadGameManager();  // Sacar esta linea cuando se instancie en el menu
    }
    void LoadGameManager()
    {
        if (!character) character = FindObjectOfType<PlayerController>().GetComponent<Character>();
        character.OnDeath += PlayerDeath;
        foreach (ReparableObject obj in toRepair)
        {
            obj.OnRepair += RepairShip;
            objectsRemaining++;
        }

        ReparableObject[] raparableObjects = FindObjectsOfType<ReparableObject>();
        toRepair.Clear();
        for (var i = 0; i < raparableObjects.Length; i++)
        {
            toRepair.Add(raparableObjects[i]);
        }
    }
    void UnloadGameManager()
    {
        if (character) character.OnDeath -= PlayerDeath;
    }
    void PlayerDeath(DamageOrigin origin)
    {
        player.playerStatus = PlayerController.PlayerStatus.EndLose;
        var gameOverText = "";
        switch (origin)
        {
            case DamageOrigin.PLAYER:
                gameOverText = "Spamming shoot kills you.\nPay attention to your overload meter to the side\nof your aim marker.";
                break;
            case DamageOrigin.RAPTOR:
                gameOverText = "Raptor are balanced dinos.\nKeep your distance and kill them one by one.";
                break;
            case DamageOrigin.TRICERATOPS:
                gameOverText = "Triceratops fling you into the air after each hit.\nTry to evade after they start charging.";
                break;
            case DamageOrigin.DILOPHOSAURUS:
                gameOverText = "Dilophosaurus shoot energy bolts.\nAttack in between their range attacks.";
                break;
            case DamageOrigin.COMPSOGNATHUS:
                gameOverText = "Compsognathus are weak by they move in groups.\nMove constantly while attacking.";
                break;
            case DamageOrigin.WATER:
                gameOverText = "Water kills robots immediately.\n Stay away from water.";
                break;
            default:
                break;
        }
        GameOver(gameOverText);
    }
    void RepairShip(RepairLocations location)
    {
        objectsRemaining--;
        if (objectsRemaining == 0)
        {
            player.playerStatus = PlayerController.PlayerStatus.EndWin;
            GameOver("You repaired the ship and got the timeline fixed.");
        }
    }
    void GameOver(string gameOverText = "")
    {
        character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ChangeScene("Menu", gameOverText);
    }
    public void ChangeScene(string scene, string gameOverText)
    {
        SceneManager.Get().LoadSceneAsync(scene, gameOverText);
    }
}

public class Global
{    
    public static bool LayerEquals(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }    
}