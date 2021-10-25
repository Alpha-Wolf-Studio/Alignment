using System;
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
    [SerializeField] private GameObject imageCircle;
    [SerializeField] private RectTransform arrow;
    RectTransform arrowStart;
    [SerializeField] private RectTransform arrowEnd;

    private float maxTimeUp = 0.2f;
    private float maxTimeDiag = 0.1f;
    private float maxTimeRight = 0.2f;
    private float maxTimeArrow = 0.35f;

    private float onTime;
    private float onTimeArrow;
    private float onTimeCircle;
    private State state = State.Up;

    private IEnumerator linesCorr;
    private IEnumerator arrowCorr;
    private IEnumerator circleCorr;
    
    private void Awake()
    {

    }

    private void Start()
    {

    }
    private void OnEnable()
    {
        Reset();
    }
    private void OnDisable()
    {

    }
    private void Update()
    {
        onTime += Time.deltaTime;
        onTimeArrow += Time.deltaTime;
        onTimeCircle += Time.deltaTime;
        float lerp = 0;

        switch (state)
        {
            case State.Up:
                lerp = onTime / maxTimeUp;
                imageUp.fillAmount = lerp;
                if (lerp > 1)
                    FinishState();
                break;
            case State.Diag:
                lerp = onTime / maxTimeDiag;
                imageDiag.fillAmount = lerp;
                if (lerp > 1)
                    FinishState(); 
                break;
            case State.Right:
                lerp = onTime / maxTimeRight;
                imageRight.fillAmount = lerp;
                if (lerp > 1)
                    FinishState(); 
                break;
            case State.Done:
                break;
        }
    }
    void FinishState()
    {
        onTime = 0;
        state++;
    }
    IEnumerator Lines()
    {
        while (true)
        {

            yield return null;
        }
    }
    private void Reset()
    {
        onTime = 0;
        imageUp.fillAmount = 0;
        imageDiag.fillAmount = 0;
        imageRight.fillAmount = 0;
        state = State.Up;
    }
}