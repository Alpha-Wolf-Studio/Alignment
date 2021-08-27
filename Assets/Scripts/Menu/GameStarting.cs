using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarting : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private GameManager gm;

    public PlayerController player;
    // Start is called before the first frame update
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        if (player.playerStatus == PlayerController.PlayerStatus.Fading)
        {
            
        }

        switch (player.playerStatus)
        {
            case PlayerController.PlayerStatus.EndWin:
                break;
            case PlayerController.PlayerStatus.EndLose:
                break;
            case PlayerController.PlayerStatus.Game:
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