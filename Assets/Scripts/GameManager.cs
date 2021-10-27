using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{

    public Entity playerEntity;
    public QuestHandler questHandler;
    public PlayerController player;
    public List<ReparableObject> toRepair;
    private int objectsRemaining;
    void Start()
    {
        LoadGameManager();  // Sacar esta linea cuando se instancie en el menu
    }
    void LoadGameManager()
    {
        if (!playerEntity) playerEntity = FindObjectOfType<PlayerController>().GetComponent<Entity>();
        playerEntity.OnDeath += PlayerDeath;
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
        if (playerEntity) playerEntity.OnDeath -= PlayerDeath;
    }
    void PlayerDeath(DamageInfo info)
    {
        if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerDie))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerDie), gameObject);

        player.ChangeStatus(PlayerController.PlayerStatus.EndLose);
        var gameOverText = UIGameOverScreen.GetGameOverText(info.origin);
        GameOver(gameOverText);
    }
    void RepairShip(RepairLocations location)
    {
        objectsRemaining--;
        if (objectsRemaining == 0)
        {
            player.ChangeStatus(PlayerController.PlayerStatus.EndWin);
            GameOver("You repaired the ship and got the timeline fixed.");
        }
    }
    void GameOver(string gameOverText = "")
    {
        playerEntity.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
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