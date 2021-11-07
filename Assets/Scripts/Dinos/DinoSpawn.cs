using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DinoSpawn : MonoBehaviour
{
    public void SetSpawn(GameObject dinoPrefab, LayerMask groundLayer, LayerMask blockLayer, float spawnTime, float minSpawnDistanceFromPlayer, Transform playerTransform) 
    {
        this.dinoPrefab = dinoPrefab;
        this.playerTransform = playerTransform;
        this.spawnTime = spawnTime;
        this.minSpawnDistanceFromPlayer = minSpawnDistanceFromPlayer;
        this.groundLayer = groundLayer;
        this.blockLayer = blockLayer;
    }

    Transform playerTransform = null;
    GameObject dinoPrefab = null;
    float spawnTime = 5f;
    float minSpawnDistanceFromPlayer = 50f;
    LayerMask groundLayer = default;
    LayerMask blockLayer = default;

    public float spawnDistanceFromCenter = 50f;
    public int dinoAmount = 0;
    public Action<DinoType> OnDinoDied;

    List<EnemyAI> enemiesIA = new List<EnemyAI>();

    private void Start()
    {
        SpawnAllDinos();
    }

    void SpawnAllDinos()
    {
        for (int i = 0; i < dinoAmount; i++)
        {
            Vector3 newPos;
            newPos = GetNewGroundRandomPosition();
            GameObject go = SpawnDino(dinoPrefab, newPos);
            SetUpAI(go);
        }
    }

    void DinoDied(DinoType type, EnemyAI ia)
    {
        enemiesIA.Remove(ia);
        OnDinoDied?.Invoke(type);
        StartCoroutine(DinoRespawn());
    }

    GameObject SpawnDino(GameObject prefab, Vector3 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity, gameObject.transform);
    }

    Vector3 GetNewGroundRandomPosition()
    {
        Vector3 randPos;
        RaycastHit groundHit;
        do
        {
            float spawnDistanceX = UnityEngine.Random.value * spawnDistanceFromCenter;
            float spawnDistanceZ = UnityEngine.Random.value * spawnDistanceFromCenter;
            Vector3 variable = new Vector3(spawnDistanceX, 0, spawnDistanceZ);
            randPos = transform.position + variable;
            randPos.y += 100f;
            Physics.Raycast(randPos, Vector3.down, out groundHit, 200);
        }
        while (blockLayer == (blockLayer | (1 << groundHit.collider.gameObject.layer)));
        Physics.Raycast(randPos, Vector3.down, out groundHit, 200, groundLayer);
        return groundHit.point;
    }

    IEnumerator DinoRespawn()
    {
        yield return new WaitForSeconds(spawnTime);
        Vector3 newPos;
        newPos = GetNewGroundRandomPosition();
        do
        {
            yield return null;
        } while (Vector3.Distance(newPos, playerTransform.position) < minSpawnDistanceFromPlayer);
        GameObject go = SpawnDino(dinoPrefab, newPos);
        SetUpAI(go);
    }

    void SetUpAI(GameObject dinoGO)
    {
        EnemyAI ai = dinoGO.GetComponent<EnemyAI>();
        ai.OnDied += DinoDied;
        ai.OnDinoRecievedDamage += FrenzyDinosaurs;
        ai.OnPlayerSpotted += FrenzyDinosaurs;
        ai.playerTransform = playerTransform;
        enemiesIA.Add(ai);
    }

    void FrenzyDinosaurs() 
    {
        foreach (var ia in enemiesIA)
        {
            ia.StartFrenzy();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, spawnDistanceFromCenter);
    }
#endif

}
