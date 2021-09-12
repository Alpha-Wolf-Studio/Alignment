using System;
using UnityEngine;
using UnityEngine.UI;

public class UiCompass : MonoBehaviour
{
    [SerializeField] private RawImage compass;
    private Transform player;

    private void Awake()
    {
        player = GameManager.Get().player.transform;
    }

    private void Update()
    {
        compass.uvRect = new Rect(player.localEulerAngles.y / 360f, 0, 1, 1);
    }
}