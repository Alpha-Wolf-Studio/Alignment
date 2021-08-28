using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public PlayerController player;

    void Start()
    {
        GameManager.Get().player = player;
        GameManager.Get().character = player.GetComponent<Character>();
    }
}