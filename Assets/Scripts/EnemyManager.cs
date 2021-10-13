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
    [SerializeField] float minSpawnDistanceFromPlayer = 50f;
    [SerializeField] LayerMask groundLayer = default;
    [SerializeField] LayerMask blockLayer = default;
    public Action<DinoClass> OnDinoDied;

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
                Vector3 newPos;
                newPos = GetNewGroundRandomPosition(spawns, i);
                GameObject go = SpawnDino(dinoPrefab, newPos, spawns, i);
                SetUpAI(go, i);
            }
        }
    }

    void DinoDied(DinoClass type, int spawnIndex) 
    {
        OnDinoDied?.Invoke(type);
        StartCoroutine(DinoRespawn(type, spawnIndex));
    }

    GameObject SpawnDino(GameObject prefab, Vector3 pos, List<DinoSpawn> spawns, int index) 
    {
        return Instantiate(prefab, pos, Quaternion.identity, spawns[index].transform);
    }

    Vector3 GetNewGroundRandomPosition(List<DinoSpawn> spawns, int index) 
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
        return groundHit.point;
    }

    IEnumerator DinoRespawn(DinoClass type, int spawnIndex) 
    {
        yield return new WaitForSeconds(spawnTime);
        GameObject spawnPrefab;
        List<DinoSpawn> listToSpawnFrom;
        switch (type)
        {
            case DinoClass.Raptor:
                listToSpawnFrom = raptorSpawns;
                spawnPrefab = raptorPrefab;
                break;
            case DinoClass.Triceratops:
                listToSpawnFrom = triSpawns;
                spawnPrefab = triPrefab;
                break;
            case DinoClass.Dilophosaurus:
                listToSpawnFrom = diloSpawns;
                spawnPrefab = diloPrefab;
                break;
            case DinoClass.Compsognathus:
                listToSpawnFrom = compiSpawns;
                spawnPrefab = compiPrefab;
                break;
            default:
                listToSpawnFrom = raptorSpawns;
                spawnPrefab = raptorPrefab;
                break;
        }
        Vector3 newPos;
        newPos = GetNewGroundRandomPosition(listToSpawnFrom, spawnIndex);
        do
        {
            yield return null;
        } while (Vector3.Distance(newPos, playerTransform.position) < minSpawnDistanceFromPlayer);
        GameObject go = SpawnDino(spawnPrefab, newPos, listToSpawnFrom, spawnIndex);

        SetUpAI(go, spawnIndex);
    }

    void SetUpAI(GameObject dinoGO, int index) 
    {
        EnemyAI ai = dinoGO.GetComponent<EnemyAI>();
        ai.OnDied += DinoDied;
        ai.SetSpawnIndex(index);
        ai.playerTransform = playerTransform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var dinoSpawn in raptorSpawns)
        {
            Global.GizmosDisk(dinoSpawn.transform.position, dinoSpawn.transform.right, dinoSpawn.spawnDistanceFromCenter);
        }
        foreach (var dinoSpawn in compiSpawns)
        {
            Global.GizmosDisk(dinoSpawn.transform.position, dinoSpawn.transform.right, dinoSpawn.spawnDistanceFromCenter);
        }
        foreach (var dinoSpawn in triSpawns)
        {
            Global.GizmosDisk(dinoSpawn.transform.position, dinoSpawn.transform.right, dinoSpawn.spawnDistanceFromCenter);
        }
        foreach (var dinoSpawn in diloSpawns)
        {
            Global.GizmosDisk(dinoSpawn.transform.position, dinoSpawn.transform.right, dinoSpawn.spawnDistanceFromCenter);
        }
    }

}
