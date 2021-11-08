using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiButtonSwapStats : MonoBehaviour, IPointerDownHandler
{
    public Transform panelToMove;
    public bool inProgress;
    public Vector3 posInitial;
    public Vector3 posEnd;
    public float onTime;
    public float swapTime = 1;
    public bool open;
    public Transform arrowsRot;
    public Image[] imageSwap;
    private Color colorOn;
    private Color colorOff;
    private Quaternion rotInitial;
    private Quaternion rotEnd;

    private void Start()
    {
        posInitial = panelToMove.localPosition;
        rotInitial = arrowsRot.transform.rotation;
        rotEnd = new Quaternion(0, 0, 1, 0);
        colorOn = Color.white;
        colorOff = Color.white;
        colorOff.a = 0;
        //posEnd.x = -2780;
        Initial();
    }
    public void Initial()
    {
        panelToMove.localPosition = posInitial;
        open = false;
        imageSwap[0].color = colorOff;
        imageSwap[1].color = colorOn;
        arrowsRot.localRotation = Quaternion.identity;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!inProgress)
        {
            StartCoroutine(SwapPanels());
        }
    }
    IEnumerator SwapPanels()
    {
        float lerp = onTime / swapTime;
        while (onTime < swapTime)
        {
            onTime += Time.unscaledDeltaTime;
            lerp = onTime / swapTime;
            if (open)
            {
                panelToMove.localPosition = Vector3.Lerp(posEnd, posInitial, lerp);
                imageSwap[0].color = Color.Lerp(colorOn, colorOff, lerp);
                imageSwap[1].color = Color.Lerp(colorOff, colorOn, lerp);
                arrowsRot.localRotation = Quaternion.Lerp(rotEnd, rotInitial, lerp);
            }
            else
            {
                panelToMove.localPosition = Vector3.Lerp(posInitial, posEnd, lerp);
                imageSwap[0].color = Color.Lerp(colorOff, colorOn, lerp);
                imageSwap[1].color = Color.Lerp(colorOn, colorOff, lerp);
                arrowsRot.localRotation = Quaternion.Lerp(rotInitial, rotEnd, lerp);
            }

            yield return null;
        }


        open = !open;
        inProgress = false;
        onTime = 0;
    }
}