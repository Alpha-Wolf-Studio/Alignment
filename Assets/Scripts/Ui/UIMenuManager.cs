using System.Collections;
using UnityEngine;
using TMPro;
public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup[] menues;
    [SerializeField] private float timeTransition;
    [SerializeField] private float textSize = 48f;
    [Space(10)]
    [SerializeField] TextMeshProUGUI versionText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        versionText.text = "Version: " + Application.version;
        Time.timeScale = 1;
    }
    public enum Menues { Main, Play, Credits, Options, Exit }
    public Menues menuActual = Menues.Main;
    private float onTime;

    public void OffPanel()
    {
        menues[(int)menuActual].blocksRaycasts = false;
        menues[(int)menuActual].interactable = false;
        StartCoroutine(PanelOff(timeTransition, (int)menuActual));
    }
    public void SwitchPanel(int otherMenu)
    {
        menues[(int) menuActual].blocksRaycasts = false;
        menues[(int) menuActual].interactable = false;
        StartCoroutine(SwitchPanel(timeTransition, otherMenu, (int) menuActual));
    }
    IEnumerator SwitchPanel(float maxTime, int onMenu, int offMenu)
    {
        CanvasGroup on = menues[onMenu];
        CanvasGroup off = menues[offMenu];

        while (onTime < maxTime)
        {
            onTime += Time.deltaTime;
            float fade = onTime / maxTime;
            on.alpha = fade;
            off.alpha = 1 - fade;
            yield return null;
        }
        on.blocksRaycasts = true;
        on.interactable = true;
        onTime = 0;

        menuActual = (Menues)onMenu;
    }
    IEnumerator PanelOff(float maxTime, int offMenu)
    {
        CanvasGroup off = menues[offMenu];

        while (onTime < maxTime)
        {
            onTime += Time.deltaTime;
            float fade = onTime / maxTime;
            off.alpha = 1 - fade;
            yield return null;
        }
        onTime = 0;
    }
    public void ChangeScene(string scene)
    {
        SceneManager.Get().LoadSceneAsync(scene, "Loading", textSize, true, RoboFaces.Warning);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}