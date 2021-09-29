using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Character playerCharacter;

    [Header("Camera Shake")]
    [SerializeField] float shakeTime = .25f;
    [SerializeField] float shakeStrenght = .5f;
    bool shaking = false;
    private void Awake()
    {
        playerCharacter.OnCharacterTakeArmorDamage += CameraShake;
        playerCharacter.OnCharacterTakeEnergyDamage += CameraShake;
    }

    void CameraShake() 
    {
        if(!shaking) 
        {
            StartCoroutine(CameraShakeCoroutine());
        }
    }

    IEnumerator CameraShakeCoroutine() 
    {
        float t = 0;
        Vector3 startingPos = transform.localPosition;
        shaking = true;
        do
        {
            t += Time.deltaTime;
            Vector3 rand = UnityEngine.Random.insideUnitSphere * shakeStrenght;
            transform.localPosition = new Vector3(startingPos.x + rand.x, startingPos.y + rand.y, startingPos.z);
            yield return null;
        } while (t < shakeTime);
        transform.localPosition = startingPos;
        shaking = false;
    }
}
