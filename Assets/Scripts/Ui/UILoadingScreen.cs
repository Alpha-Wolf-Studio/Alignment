using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] Image blackScreen = null;
    [SerializeField] Image loadingBar = null;
    [SerializeField] Image loadingBarOverlay = null;
    [SerializeField] TextMeshProUGUI textComponent = null;

    [Header("Roboface")]
    [SerializeField] Image roboFace = null;
    [SerializeField] Sprite[] possibleRoboFaces;
    public void SetNewRoboface(RoboFaces newRoboFace)
    {
        if (newRoboFace != RoboFaces.None) roboFace.sprite = possibleRoboFaces[(int)newRoboFace];
    }

    bool canFadeOut = true;

    public void FadeWithBlackScreen(string text = "", bool useLoadingBar = false, bool useRoboFace = false)
    {
        StopAllCoroutines();
        textComponent.text = text;
        StartCoroutine(blackScreenFade(useLoadingBar, useRoboFace));
    }

    IEnumerator blackScreenFade(bool useLoadingBar, bool useRoboFace)
    {
        while (blackScreen.color.a + Time.unscaledDeltaTime < 1)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + Time.unscaledDeltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a + Time.unscaledDeltaTime);
            if (useLoadingBar) 
            {
                loadingBar.color = new Color(loadingBar.color.r, loadingBar.color.g, loadingBar.color.b, loadingBar.color.a + Time.unscaledDeltaTime);
                loadingBarOverlay.color = new Color(loadingBarOverlay.color.r, loadingBarOverlay.color.g, loadingBarOverlay.color.b, loadingBarOverlay.color.a + Time.unscaledDeltaTime);
            }
            if(useRoboFace) 
            {
                roboFace.color = new Color(roboFace.color.r, roboFace.color.g, roboFace.color.b, roboFace.color.a + Time.unscaledDeltaTime);
            }
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1);
        if (useLoadingBar)
        {
            loadingBar.color = new Color(loadingBar.color.r, loadingBar.color.g, loadingBar.color.b, 1);
            loadingBarOverlay.color = new Color(loadingBarOverlay.color.r, loadingBarOverlay.color.g, loadingBarOverlay.color.b, 1);
        }
        if (useRoboFace)
        {
            roboFace.color = new Color(roboFace.color.r, roboFace.color.g, roboFace.color.b, 1);
        }
        while (!canFadeOut)
        {
            yield return null;
        }
        while (blackScreen.color.a - Time.unscaledDeltaTime > 0)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - Time.unscaledDeltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a - Time.unscaledDeltaTime);
            if (useLoadingBar)
            {
                loadingBar.color = new Color(loadingBar.color.r, loadingBar.color.g, loadingBar.color.b, loadingBar.color.a - Time.unscaledDeltaTime);
                loadingBarOverlay.color = new Color(loadingBarOverlay.color.r, loadingBarOverlay.color.g, loadingBarOverlay.color.b, loadingBarOverlay.color.a - Time.unscaledDeltaTime);
            }
            if (useRoboFace)
            {
                roboFace.color = new Color(roboFace.color.r, roboFace.color.g, roboFace.color.b, roboFace.color.a - Time.unscaledDeltaTime);
            }
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
        if (useLoadingBar)
        {
            loadingBar.color = new Color(loadingBar.color.r, loadingBar.color.g, loadingBar.color.b, 0);
            loadingBarOverlay.color = new Color(loadingBarOverlay.color.r, loadingBarOverlay.color.g, loadingBarOverlay.color.b, 0);
        }
        if (useRoboFace)
        {
            roboFace.color = new Color(roboFace.color.r, roboFace.color.g, roboFace.color.b, 0);
        }
    }

    public void LockFade()
    {
        canFadeOut = false;
    }

    public void UnlockFade()
    {
        canFadeOut = true;
    }

    public void UpdateLoadingBar(float barAmount) 
    {
        loadingBar.fillAmount = barAmount;
    }

    public void SetTextSize(float size) 
    {
        textComponent.fontSize = size;
    }

    public void BlackScreenUnfade(bool useLoadingBar = false, bool useRoboFace = false)
    {
        StartCoroutine(blackScreenUnfadeCoroutine(useLoadingBar, useRoboFace));
    }

    IEnumerator blackScreenUnfadeCoroutine(bool useLoadingBar, bool useRoboFace)
    {
        while (blackScreen.color.a - Time.unscaledDeltaTime > 0)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - Time.unscaledDeltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a - Time.unscaledDeltaTime);
            if (useLoadingBar)
            {
                loadingBar.color = new Color(loadingBar.color.r, loadingBar.color.g, loadingBar.color.b, loadingBar.color.a - Time.unscaledDeltaTime);
                loadingBarOverlay.color = new Color(loadingBarOverlay.color.r, loadingBarOverlay.color.g, loadingBarOverlay.color.b, loadingBarOverlay.color.a - Time.unscaledDeltaTime);
            }
            if (useRoboFace)
            {
                roboFace.color = new Color(roboFace.color.r, roboFace.color.g, roboFace.color.b, roboFace.color.a - Time.unscaledDeltaTime);
            }
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
        if (useLoadingBar)
        {
            loadingBar.color = new Color(loadingBar.color.r, loadingBar.color.g, loadingBar.color.b, 0);
            loadingBarOverlay.color = new Color(loadingBarOverlay.color.r, loadingBarOverlay.color.g, loadingBarOverlay.color.b, 0);
        }
        if (useRoboFace)
        {
            roboFace.color = new Color(roboFace.color.r, roboFace.color.g, roboFace.color.b, 0);
        }
    }
}
public enum RoboFaces
{
    Happy,
    Dead,
    Warning,
    Message,
    Question,
    Sad,
    None
};
