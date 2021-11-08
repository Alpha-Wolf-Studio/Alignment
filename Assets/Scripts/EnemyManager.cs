using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Transform playerTransform = null;
    [SerializeField] float spawnTime = 5f;
    [SerializeField] float minSpawnDistanceFromPlayer = 50f;
    [SerializeField] LayerMask groundLayer = default;
    [SerializeField] LayerMask blockLayer = default;
    public Action<DinoType> OnDinoDied;

    [Header("Robo Raptor")]
    [SerializeField] GameObject raptorPrefab = null;
    [SerializeField] List<DinoSpawn> raptorSpawns = null;
    [Header("Composgnatus")]
    [SerializeField] GameObject compiPrefab = null;
    [SerializeField] List<DinoSpawn> compiSpawns = null;
    [Header("Dilophosaurus")]
    [SerializeField] GameObject diloPrefab = null;
    [SerializeField] List<DinoSpawn> diloSpawns = null;
    [Header("Triceratops")]
    [SerializeField] GameObject triPrefab = null;
    [SerializeField] List<DinoSpawn> triSpawns = null;

    private void Awake()
    {
        SetUpSpawns(raptorPrefab, raptorSpawns);
        SetUpSpawns(compiPrefab, compiSpawns);
        SetUpSpawns(diloPrefab, diloSpawns);
        SetUpSpawns(triPrefab, triSpawns);
    }

    void SetUpSpawns(GameObject prefab, List<DinoSpawn> spawns) 
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            spawns[i].SetSpawn(prefab, groundLayer, blockLayer, spawnTime, minSpawnDistanceFromPlayer, playerTransform);
            spawns[i].OnDinoDied += DinoDied;
        }
    }

    void DinoDied(DinoType type)
    {
        OnDinoDied?.Invoke(type);
    }

}
