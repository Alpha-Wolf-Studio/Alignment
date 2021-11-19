using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEvents : MonoBehaviour
{
    [SerializeField] EnemyAI enemyAI;
    DinoType dinoType;
    uint stepAudioID = 0;

    private void Awake()
    {
        dinoType = enemyAI.dinoType;
        switch (dinoType)
        {
            case DinoType.Raptor:
                stepAudioID = AK.EVENTS.RAPTORSTEP;
                break;
            case DinoType.Triceratops:
                stepAudioID = AK.EVENTS.TRIKESTEP;
                break;
            case DinoType.Dilophosaurus:
                stepAudioID = AK.EVENTS.DILOSTEP;
                break;
            case DinoType.Compsognathus:
                stepAudioID = AK.EVENTS.COMPYSTEP;
                break;
            default:
                break;
        }
    }

    void OnStepEvent() 
    {
        AkSoundEngine.PostEvent(stepAudioID, gameObject);
    }
}
