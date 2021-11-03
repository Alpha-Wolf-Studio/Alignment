using System;
using UnityEngine;
public class PlayerPause : PlayerState
{
    private PlayerController player;

    private void Awake()
    {
        player= GetComponent<PlayerController>();
    }
    private void Update()
    {
        player.TryPause(false);
    }
}