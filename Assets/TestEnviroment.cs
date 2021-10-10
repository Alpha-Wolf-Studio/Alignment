using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnviroment : MonoBehaviour
{
    public LayerMask player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Global.LayerEquals(player, other.gameObject.layer))
        {
            Debug.Log("Player entró.");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (Global.LayerEquals(player, other.gameObject.layer))
        {
            Debug.Log("Player Salió.");
        }
    }
}
