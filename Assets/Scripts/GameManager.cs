using UnityEngine;
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public Entity playerEntity;
    public QuestManager questHandler;
    public PlayerController player;

    void Start()
    {
        GameInPause(false);
        QuestManager.Get().OnRepairedShip += CompletedGame;
        LoadGameManager();  // Sacar esta linea cuando se instancie en el menu
    }
    void LoadGameManager()
    {
        if (!playerEntity) playerEntity = FindObjectOfType<PlayerController>().GetComponent<Entity>();
        playerEntity.OnDeath += PlayerDeath;
    }
    void UnloadGameManager()
    {
        if (playerEntity) playerEntity.OnDeath -= PlayerDeath;
    }
    void PlayerDeath(DamageInfo info)
    {
        if (Sfx.Get().GetEnable(Sfx.ListSfx.PlayerDie))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PlayerDie), gameObject);

        player.ChangeControllerToNone();
        var gameOverText = UIGameOverScreen.GetGameOverText(info.origin);
        GameOver(gameOverText);
    }
    void CompletedGame() 
    {
        player.ChangeControllerToNone();
        GameOver("You repaired the ship and got the timeline fixed.");
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
    public void GameInPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }
}

public class Global
{    
    public static bool LayerEquals(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    } 
}