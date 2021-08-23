using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public enum GameStatus {Menu, Inization, Game, EndWin, EndLose }
    public GameStatus gameStatus = GameStatus.Inization;
    public Character player;
    public List<ReparableObject> toRepair = new List<ReparableObject>();
    private int objectsRemaining;

    void Start()
    {
        LoadGameManager();  // Sacar esta linea cuando se instancie en el menu
    }
    void LoadGameManager()
    {
        if (!player) player = FindObjectOfType<PlayerController>().GetComponent<Character>();
        player.OnDeath += PlayerDeath;
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
        if (player) player.OnDeath -= PlayerDeath;
    }
    void PlayerDeath()
    {
        gameStatus = GameStatus.EndLose;
        GameOver();
    }
    void RepairShip()
    {
        objectsRemaining--;
        if (objectsRemaining == 0)
        {
            gameStatus = GameStatus.EndWin;
            GameOver();
        }
    }
    void GameOver()
    {
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}

public class Global
{    
    public static bool LayerEquals(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }    
}