using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Transform playerTransform = null;
    [SerializeField] float spawnTime = 5f;
    [SerializeField] LayerMask groundLayer = default;
    [SerializeField] LayerMask blockLayer = default;
    public Action<DinoClass> OnDinoDied;

    [Serializable]
    public class DinoSpawn
    {
        public Transform transform = null;
        public float spawnDistanceFromCenter = 50f;
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
                SetUpAI(go, i);
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
        Vector3 randPos;
        RaycastHit groundHit;
        do
        {
            float spawnDistanceX = UnityEngine.Random.value * spawns[index].spawnDistanceFromCenter;
            float spawnDistanceZ = UnityEngine.Random.value * spawns[index].spawnDistanceFromCenter;
            Vector3 variable = new Vector3(spawnDistanceX, 0, spawnDistanceZ);
            randPos = spawns[index].transform.position + variable;
            randPos.y += 100f;
            Physics.Raycast(randPos, Vector3.down, out groundHit, 200);
        }
        while (blockLayer == (blockLayer | (1 << groundHit.collider.gameObject.layer))); 
        Physics.Raycast(randPos, Vector3.down, out groundHit, 200, groundLayer);
        return Instantiate(prefab, groundHit.point, Quaternion.identity, spawns[index].transform);
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
        SetUpAI(go, spawnIndex);
    }

    void SetUpAI(GameObject dinoGO, int index) 
    {
        EnemyAI ai = dinoGO.GetComponent<EnemyAI>();
        ai.OnDied += DinoDied;
        ai.SetSpawnIndex(index);
        ai.playerTransform = playerTransform;
    }

}
