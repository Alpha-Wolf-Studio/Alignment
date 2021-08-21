using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public Character player;
    void Start()
    {
        player.OnDeath += GameOver;
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