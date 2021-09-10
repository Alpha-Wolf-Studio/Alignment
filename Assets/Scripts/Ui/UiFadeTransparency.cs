using UnityEngine;
using UnityEngine.UI;

public class UiFadeTransparency : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float loopTimeMax = 2;
    [SerializeField] Color colorInitial = Color.white;
    [SerializeField] Color colorEnd = Color.white;

    private float onLoopTime;
    private bool downFade;

    private void Start()
    {
        if (!image) 
            image = GetComponent<Image>();
    }

    void Update()
    {
        onLoopTime += Time.unscaledDeltaTime;
        float lerp = onLoopTime / loopTimeMax;

        if (downFade)
        {
            image.color = Color.Lerp(colorEnd, colorInitial, lerp);
            if (onLoopTime > loopTimeMax) 
                Reset();
        }
        else
        {
            image.color = Color.Lerp(colorInitial, colorEnd, lerp);
            if (onLoopTime > loopTimeMax)
                Reset();
        }
    }
    void Reset()
    {
        onLoopTime = 0;
        downFade = !downFade;
    }
}