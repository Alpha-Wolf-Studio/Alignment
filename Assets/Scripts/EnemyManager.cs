using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] float spawnDistanceFromCenter = 50f;
    [SerializeField] float spawnTime = 5f;
    public Action<DinoClass> OnDinoDied;

    [Serializable]
    public class DinoSpawn
    {
        public Transform transform = null;
        public int dinoAmount = 0; 
    }

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

    private void Start()
    {
        SpawnAllDinosFromSpawnList(raptorSpawns, raptorPrefab);
        SpawnAllDinosFromSpawnList(compiSpawns, compiPrefab);
        SpawnAllDinosFromSpawnList(diloSpawns, diloPrefab);
        SpawnAllDinosFromSpawnList(triSpawns, triPrefab);
    }

    void SpawnAllDinosFromSpawnList(List<DinoSpawn> spawns, GameObject dinoPrefab) 
    {
        for (int i = 0; i < spawns.Count; i++)
        {
            for (int j = 0; j < spawns[i].dinoAmount; j++)
            {
                GameObject go = SpawnDino(dinoPrefab, spawns, i);
                EnemyAI ai = go.GetComponent<EnemyAI>();
                ai.OnDied += DinoDied;
                ai.SetSpawnIndex(i);
            }
        }
    }

    void DinoDied(DinoClass type, int spawnIndex) 
    {
        OnDinoDied?.Invoke(type);
        StartCoroutine(DinoRespawn(type, spawnIndex));
    }

    GameObject SpawnDino(GameObject prefab, List<DinoSpawn> spawns, int index) 
    {
        Vector3 variable = new Vector3(UnityEngine.Random.value * spawnDistanceFromCenter, 0, UnityEngine.Random.value * spawnDistanceFromCenter);
        Vector3 spawnPos = spawns[index].transform.position + variable;
        return Instantiate(prefab, spawnPos, Quaternion.identity, spawns[index].transform);
    }

    IEnumerator DinoRespawn(DinoClass type, int spawnIndex) 
    {
        yield return new WaitForSeconds(spawnTime);
        GameObject go = new GameObject();
        switch (type)
        {
            case DinoClass.Raptor:
                go = SpawnDino(raptorPrefab, raptorSpawns, spawnIndex);
                break;
            case DinoClass.Triceratops:
                go = SpawnDino(triPrefab, triSpawns, spawnIndex);
                break;
            case DinoClass.Dilophosaurus:
                go = SpawnDino(diloPrefab, diloSpawns, spawnIndex);
                break;
            case DinoClass.Compsognathus:
                go = SpawnDino(compiPrefab, compiSpawns, spawnIndex);
                break;
        }
        EnemyAI ai = go.GetComponent<EnemyAI>();
        ai.OnDied += DinoDied;
        ai.SetSpawnIndex(spawnIndex);
    }

}
