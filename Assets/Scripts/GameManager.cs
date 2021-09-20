using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    void PlayerDeath()
    {
        player.playerStatus = PlayerController.PlayerStatus.EndLose;
        GameOver();
    }
    void RepairShip(RepairLocations location)
    {
        objectsRemaining--;
        if (objectsRemaining == 0)
        {
            player.playerStatus = PlayerController.PlayerStatus.EndWin;
            GameOver();
        }
    }
    void GameOver()
    {
        character.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ChangeScene("Menu");
    }
    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

public class Global
{    
    public static bool LayerEquals(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }    
}