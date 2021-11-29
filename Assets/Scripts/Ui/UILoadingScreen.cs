﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] Image blackScreen = null;
    [SerializeField] Image loadingBar = null;
    [SerializeField] Image[] loadingImages = null;
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
        if (useLoadingBar) blackScreen.color = new Color(1, 1, 1, 0);
        else blackScreen.color = new Color(0, 0, 0, 0);
        while (blackScreen.color.a + Time.unscaledDeltaTime < 1)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + Time.unscaledDeltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a + Time.unscaledDeltaTime);
            if (useLoadingBar) 
            {
                foreach (var img in loadingImages)
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + Time.unscaledDeltaTime);
                }
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
            foreach (var img in loadingImages)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
            }
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
                foreach (var img in loadingImages)
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - Time.unscaledDeltaTime);
                }
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
            foreach (var img in loadingImages)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
            }
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
                foreach (var img in loadingImages)
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - Time.unscaledDeltaTime);
                }
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
            foreach (var img in loadingImages)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
            }
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
