using System.Collections;
using UnityEngine;

public class SceneManager : MonoBehaviourSingleton<SceneManager>
{

    [SerializeField] float minTimeToLoadScene = 1f;
    [SerializeField] float timeBeforeSceneChange = 1f;
    [SerializeField] UILoadingScreen uI_LoadingScreen = null;
    public void LoadSceneAsync(string sceneName, string textInBetween = "")
    {
        StartCoroutine(AsynchronousLoadWithFake(sceneName, textInBetween));
    }

    IEnumerator AsynchronousLoadWithFake(string scene, string textInBetween)
    {
        float loadingProgress;
        float timeLoading = 0;

        uI_LoadingScreen.FadeWithBlackScreen(textInBetween);
        uI_LoadingScreen.LockFade();
        var t = timeBeforeSceneChange;
        while (t > 0)
        {
            t -= Time.unscaledDeltaTime;
            yield return null;
        }

        AsyncOperation ao = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            timeLoading += Time.unscaledDeltaTime;
            loadingProgress = ao.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minTimeToLoadScene;

            // Se completo la carga
            if (loadingProgress >= 1)
            {
                ao.allowSceneActivation = true;
            }
            yield return null;
        }

        while (t < 1)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        uI_LoadingScreen.UnlockFade();
    }

    public void BlackScreenUnfade()
    {
        uI_LoadingScreen.BlackScreenUnfade();
    }

    public void FakeLoad(float time)
    {
        StartCoroutine(FakeLoadingWithBlackScreen(time));
    }

    public void FakeLoad(float time, string text)
    {
        StartCoroutine(FakeLoadingWithBlackScreen(time, text));
    }

    IEnumerator FakeLoadingWithBlackScreen(float time)
    {
        uI_LoadingScreen.FadeWithBlackScreen();
        uI_LoadingScreen.LockFade();
        yield return new WaitForSecondsRealtime(time);
        uI_LoadingScreen.UnlockFade();
    }

    IEnumerator FakeLoadingWithBlackScreen(float time, string text)
    {
        uI_LoadingScreen.FadeWithBlackScreen(text);
        uI_LoadingScreen.LockFade();
        yield return new WaitForSecondsRealtime(time);
        uI_LoadingScreen.UnlockFade();
    }
}
