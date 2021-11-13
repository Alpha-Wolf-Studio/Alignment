using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    public Image filledImageCoolDown;
    public Image filledImageEnergy;
    public Image filledImageArmor;
    public Image filledImageStamina;
    public Color CoolDownReloading = Color.red;
    [HideInInspector] public Entity entity;
    [HideInInspector] public PlayerController player;
    public RectTransform sightHud;
    private float speed = 10f;
    private float onTimeCD;
    private float onTimeFadePause;
    private float fadePause = 1;

    public enum CanvasGroupList { GamePlay, Pause, Options }
    public CanvasGroupList menuActual;
    public List<CanvasGroup> canvasGroup = new List<CanvasGroup>();
    [SerializeField] private GameObject console;
    [SerializeField] private GameObject panelQuest;
    [SerializeField] TextMeshProUGUI versionText;

    private void Awake()
    {
        entity = GameManager.Get().playerEntity;
        player = GameManager.Get().player;
    }
    void Start()
    {
        player.ChangeControllerToNone();
        entity.OnUpdateStats += TakeDamage;
        entity.OnDeath += Death;
        ArmRange.onShoot += Shoot;
        player.onPause += Pause;
        console.GetComponent<Console>().onOpenConsole += OpenConsole;
        player.playerGame.onOpenQuestPanel += OpenQuestPanel;

        versionText.text = "Version: " + Application.version;
    }
    void Update()
    {
        sightHud.Rotate(Vector3.forward * (speed * Time.deltaTime));
        filledImageStamina.fillAmount = player.GetCurrentStamina() / player.GetMaxStamina();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OpenConsole();
        }
    }
    void TakeDamage()
    {
        if (entity.entityStats.GetStat(StatType.Energy).GetMax() > 0)
        {
            float currentEnergy = entity.entityStats.GetStat(StatType.Energy).GetCurrent();
            float maxEnergy = entity.entityStats.GetStat(StatType.Energy).GetMax();
            filledImageEnergy.fillAmount = currentEnergy / maxEnergy;
        }
        if (entity.entityStats.GetStat(StatType.Armor).GetMax() > 0)
        {
            float currentArmor = entity.entityStats.GetStat(StatType.Armor).GetCurrent();
            float maxArmor = entity.entityStats.GetStat(StatType.Armor).GetMax();
            filledImageArmor.fillAmount = currentArmor / maxArmor;
        }
    }
    void Shoot(float maxCoolDown, bool reloading)
    {
        if (reloading)
        {
            StartCoroutine(Reloading(maxCoolDown));
        }
    }
    void Death(DamageInfo info)
    {

    }
    void OpenConsole()
    {
        console.SetActive(!console.activeSelf);
    }
    void OpenQuestPanel()
    {
        panelQuest.SetActive(!panelQuest.activeSelf);
    }
    public void Pause(bool isPause)
    {
        if (isPause)
        {
            StartCoroutine(PauseEnabling());
        }
        else
        {
            StartCoroutine(PauseDisabling());
        }
    }
    IEnumerator Reloading(float maxCD)
    {
        filledImageCoolDown.color = CoolDownReloading;
        while (onTimeCD < maxCD)
        {
            onTimeCD += Time.deltaTime;
            filledImageCoolDown.fillAmount = onTimeCD / maxCD;
            yield return null;
        }

        filledImageCoolDown.color = Color.white;
        onTimeCD = 0;
    }
    IEnumerator PauseEnabling()
    {
        if (Sfx.Get().GetEnable(Sfx.ListSfx.PauseOn))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PauseOn), gameObject);
        GameManager.Get().GameInPause(false);
        player.ChangeControllerToNone();
        onTimeFadePause = 0;
        canvasGroup[(int)CanvasGroupList.Pause].alpha = 0;
        canvasGroup[(int)CanvasGroupList.Options].alpha = 0;
        EnableCanvasGroup(canvasGroup[(int)CanvasGroupList.GamePlay], false);
        EnableCanvasGroup(canvasGroup[(int)CanvasGroupList.Pause], false);
        EnableCanvasGroup(canvasGroup[(int)CanvasGroupList.Options], false);

        while (onTimeFadePause < fadePause)
        {
            onTimeFadePause += Time.unscaledDeltaTime;
            float fade = onTimeFadePause / fadePause;
            canvasGroup[(int) CanvasGroupList.Pause].alpha = fade;
            canvasGroup[(int) menuActual].alpha = 1 - fade;

            yield return null;
        }

        canvasGroup[(int) CanvasGroupList.Pause].interactable = true;
        canvasGroup[(int) CanvasGroupList.Pause].blocksRaycasts = true;

        onTimeFadePause = 0;
        player.ChangeControllerToPause();
        player.AvailableCursor(true);
        menuActual = CanvasGroupList.Pause;
    }
    IEnumerator PauseDisabling()
    {
        player.ChangeControllerToNone();
        onTimeFadePause = 0;

        EnableCanvasGroup(canvasGroup[(int)CanvasGroupList.Pause], false);
        EnableCanvasGroup(canvasGroup[(int)CanvasGroupList.Options], false);

        while (onTimeFadePause < fadePause)
        {
            onTimeFadePause += Time.unscaledDeltaTime;
            float fade = onTimeFadePause / fadePause;
            canvasGroup[(int) menuActual].alpha = 1 - fade;
            canvasGroup[(int) CanvasGroupList.GamePlay].alpha = fade;

            yield return null;
        }

        canvasGroup[(int)CanvasGroupList.Pause].alpha = 0;
        canvasGroup[(int)CanvasGroupList.Options].alpha = 0;
        EnableCanvasGroup(canvasGroup[(int) CanvasGroupList.GamePlay], true);
        onTimeFadePause = 0;
        player.ChangeControllerToGame();
        GameManager.Get().GameInPause(false);
        player.AvailableCursor(false);
        menuActual = CanvasGroupList.GamePlay;
        if (Sfx.Get().GetEnable(Sfx.ListSfx.PauseOff))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.PauseOff), gameObject);
    }
    public void SwitchPanel(int otherMenu)
    {
        canvasGroup[(int)menuActual].blocksRaycasts = false;
        canvasGroup[(int)menuActual].interactable = false;
        StartCoroutine(SwitchPanel(fadePause, otherMenu, (int)menuActual));
    }
    IEnumerator SwitchPanel(float maxTime, int onMenu, int offMenu)
    {
        CanvasGroup on = canvasGroup[onMenu];
        CanvasGroup off = canvasGroup[offMenu];

        while (onTimeFadePause < maxTime)
        {
            onTimeFadePause += Time.unscaledDeltaTime;
            float fade = onTimeFadePause / maxTime;
            on.alpha = fade;
            off.alpha = 1 - fade;
            yield return null;
        }

        EnableCanvasGroup(on, true);
        onTimeFadePause = 0;

        menuActual = (CanvasGroupList)onMenu;
    }
    void EnableCanvasGroup(CanvasGroup canvasGroup, bool enable)
    {
        canvasGroup.interactable = enable;
        canvasGroup.blocksRaycasts = enable;
    }
    public void ChangeScene(string scene)
    {
        SceneManager.Get().LoadSceneAsync(scene);
    }
}