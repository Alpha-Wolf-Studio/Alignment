using UnityEngine;
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public Entity playerEntity;
    public QuestManager questHandler;
    public PlayerController player;

    private void Start()
    {
        GameInPause(false);
        QuestManager.Get().OnRepairedShip += CompletedGame;
        LoadGameManager();  // Sacar esta linea cuando se instancie en el menu
    }
    private void LoadGameManager()
    {
        if (!playerEntity) playerEntity = FindObjectOfType<PlayerController>().GetComponent<Entity>();
        playerEntity.OnDeath += PlayerDeath;
    }
    private void UnloadGameManager()
    {
        if (playerEntity) playerEntity.OnDeath -= PlayerDeath;
    }
    private void PlayerDeath(DamageInfo info)
    {
        AkSoundEngine.PostEvent(AK.EVENTS.PLAYERDIE, gameObject);

        player.ChangeControllerToNone();
        var gameOverText = UIGameOverScreen.GetGameOverText(info.origin);
        GameOver(gameOverText);
    }
    private void CompletedGame() 
    {
        player.ChangeControllerToNone();
        GameOver("You repaired the ship and got the timeline fixed.");
    }
    private void GameOver(string gameOverText = "")
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