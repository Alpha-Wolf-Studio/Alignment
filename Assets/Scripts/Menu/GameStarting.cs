using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarting : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private GameManager gm;
    // Start is called before the first frame update
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        if (gm.gameStatus == GameManager.GameStatus.Menu)
        {
            
        }

        switch (gm.gameStatus)
        {
            case GameManager.GameStatus.EndWin:
                break;
            case GameManager.GameStatus.EndLose:
                break;
            case GameManager.GameStatus.Game:
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}