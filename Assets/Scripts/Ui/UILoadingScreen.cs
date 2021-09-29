using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoadingScreen : MonoBehaviour
{
    [SerializeField] Image blackScreen = null;
    [SerializeField] TextMeshProUGUI textComponent = null;

    bool canFadeOut = true;

    public void FadeWithBlackScreen()
    {
        StopAllCoroutines();
        textComponent.text = "";
        StartCoroutine(blackScreenFade());
    }

    public void FadeWithBlackScreen(string text)
    {
        StopAllCoroutines();
        textComponent.text = text;
        StartCoroutine(blackScreenFade());
    }

    IEnumerator blackScreenFade()
    {
        while (blackScreen.color.a + Time.unscaledDeltaTime < 1)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + Time.unscaledDeltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, textComponent.color.a + Time.unscaledDeltaTime);
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 1);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1);
        while (!canFadeOut)
        {
            yield return null;
        }
        while (blackScreen.color.a - Time.unscaledDeltaTime > 0)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - Time.unscaledDeltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, blackScreen.color.a - Time.unscaledDeltaTime);
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
    }

    public void LockFade()
    {
        canFadeOut = false;
    }

    public void UnlockFade()
    {
        canFadeOut = true;
    }

    public void BlackScreenUnfade()
    {
        StartCoroutine(blackScreenUnfadeCoroutine());
    }

    IEnumerator blackScreenUnfadeCoroutine()
    {
        while (blackScreen.color.a - Time.unscaledDeltaTime > 0)
        {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a - Time.unscaledDeltaTime);
            textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, blackScreen.color.a - Time.unscaledDeltaTime);
            yield return null;
        }
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0);
    }
}
