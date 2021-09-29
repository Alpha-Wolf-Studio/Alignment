using System;
using UnityEngine;
public class UiWorldFadeByDistance : MonoBehaviour
{
    public Action<bool> onActive;

    [SerializeField] private Transform myTransform;
    public Transform otherTransform;
    [SerializeField] private CanvasGroup canvasGroupFade;

    [SerializeField] private float minDistanceToShow = 5;
    [SerializeField] private float maxDistanceToShow = 15;
    [SerializeField] private bool enable;

    private void Start()
    {
        SetIfNull();
        Enabled(false);
    }
    private void Update()
    {
        EnablePanel();
    }
    private void SetIfNull()
    {
        if (!myTransform)
        {
            myTransform = transform;
            Debug.LogWarning("No está seteado myTransform: ", gameObject);
        }
        if (!otherTransform)
        {
            otherTransform = GameManager.Get().player.transform;
        }
        if (!canvasGroupFade)
        {
            canvasGroupFade = GetComponentInChildren<CanvasGroup>();
            Debug.LogWarning("No está seteado canvasGroupFade: ", gameObject);
        }
        if (minDistanceToShow == 0 | maxDistanceToShow == 0)
        {
            Debug.LogWarning("minDistanceToShow = " + minDistanceToShow + " maxDistanceToShow = " + maxDistanceToShow, gameObject);
        }
    }
    private void EnablePanel()
    {
        float distanceSqr = Vector3.SqrMagnitude(otherTransform.position - myTransform.position);
        if (distanceSqr < maxDistanceToShow * maxDistanceToShow)
        {
            float distance = DistanceToPlayer();
            canvasGroupFade.alpha = 1 - (distance - minDistanceToShow) / (maxDistanceToShow - minDistanceToShow);
            if (!enable)
            {
                Enabled(true);
            }
        }
        else
        {
            if (enable)
            {
                Enabled(false);
            }
        }
    }
    private float DistanceToPlayer() => Vector3.Distance(myTransform.position, otherTransform.position);
    private void Enabled(bool on)
    {
        onActive?.Invoke(on);
        enable = on;
        canvasGroupFade.blocksRaycasts = on;
        canvasGroupFade.interactable = on;
    }
}