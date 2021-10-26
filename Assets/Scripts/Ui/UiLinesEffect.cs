using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UiLinesEffect : MonoBehaviour
{
    enum State
    {
        Up,
        Diag,
        Right,
        Done
    }
    [SerializeField] private Image imageUp;
    [SerializeField] private Image imageDiag;
    [SerializeField] private Image imageRight;
    [SerializeField] private RectTransform imageCircle;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform arrowFinal;
    private Vector2 arrowStart;
    private Vector2 arrowEnd;

    private float maxScaleCircle = 3;
    private float maxTimeUp = 0.2f;
    private float maxTimeDiag = 0.1f;
    private float maxTimeRight = 0.2f;
    private float maxTimeArrow = 0.35f;
    private float maxTimeCirUp = 0.4f;
    private float maxTimeCirDown = 0.3f;

    private float onTime;
    private float onTimeArrow;
    private float onTimeCircle;

    private Quaternion startRotation;

    private void Awake()
    {
        arrowStart = arrow.anchoredPosition;
        arrowEnd = arrowFinal.anchoredPosition;
    }
    private void OnEnable()
    {
        Reset();
        StartCoroutine(LineConstructor());
        StartCoroutine(ArrowMove());
        StartCoroutine(CircleExpand());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator LineConstructor()
    {
        onTime = 0;
        while (onTime < maxTimeUp) // Line Up
        {
            onTime += Time.deltaTime;
            float lerp = onTime / maxTimeUp;
            imageUp.fillAmount = lerp;
            yield return null;
        }
        onTime = 0;
        while (onTime < maxTimeDiag) // Line Diag
        {
            onTime += Time.deltaTime;
            float lerp = onTime / maxTimeDiag;
            imageDiag.fillAmount = lerp;
            yield return null;
        }
        onTime = 0;
        while (onTime < maxTimeRight) // Line Right
        {
            onTime += Time.deltaTime;
            float lerp = onTime / maxTimeRight;
            imageRight.fillAmount = lerp;
            yield return null;
        }
        onTime = 0;
    }
    IEnumerator ArrowMove()
    {
        onTimeArrow = 0;
        while (onTimeArrow < maxTimeArrow) // Line Up
        {
            onTimeArrow += Time.deltaTime;
            float lerp = onTimeArrow / maxTimeArrow;
            arrow.anchoredPosition = Vector2.Lerp(arrowStart, arrowEnd, lerp);
            yield return null;
        }
    }
    IEnumerator CircleExpand()
    {
        onTimeCircle = 0;
        while (onTimeCircle < maxTimeCirUp)         // Expand Circle
        {
            onTimeCircle += Time.deltaTime;
            float lerp = onTimeCircle / maxTimeCirUp;
            imageCircle.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * maxScaleCircle, lerp);
            yield return null;
        }
        onTimeCircle = 0;
        while (onTimeCircle < maxTimeCirDown)       // Retract Circle
        {
            onTimeCircle += Time.deltaTime;
            float lerp = onTimeCircle / maxTimeCirDown;
            imageCircle.localScale = Vector3.Lerp(Vector3.one * maxScaleCircle, Vector3.one, lerp);
            yield return null;
        }
    }
    private void Reset()
    {
        onTime = 0;
        onTimeArrow = 0;
        onTimeCircle = 0;
        imageUp.fillAmount = 0;
        imageDiag.fillAmount = 0;
        imageRight.fillAmount = 0;
        arrow.anchoredPosition = arrowStart;
        imageCircle.localScale = Vector3.one;
    }
}