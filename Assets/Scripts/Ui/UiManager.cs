using System;
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
    [HideInInspector] public Character character;
    [HideInInspector] public PlayerController player;
    public RectTransform sightHud;
    private float speed = 0.1f;
    private float onTimeCD;
    private float onTimeFadePause;
    private float fadePause = 1;

    public enum CanvasGroupList { GamePlay, Pause, Options }
    public CanvasGroupList menuActual;
    public List<CanvasGroup> canvasGroup = new List<CanvasGroup>();
    private bool fromInventory;
    [SerializeField] private GameObject console;
    [SerializeField] private GameObject panelQuest;
    [SerializeField] TextMeshProUGUI versionText;

    private void Awake()
    {
        character = GameManager.Get().character;
        player = GameManager.Get().player;
    }

    void Start()
    {
        player.playerStatus = PlayerController.PlayerStatus.Inization;
        character.OnUpdateStats += TakeDamage;
        character.OnDeath += Death;
        player.onShoot+= Shoot;
        player.OnPause+= Pause;
        player.OnOpenConsole += OpenConsole;
        player.OnOpenQuestPanel += OpenQuestPanel;
        Time.timeScale = 1;

        versionText.text = "Version: " + Application.version;
    }
    void Update()
    {
        sightHud.Rotate(Vector3.forward * (speed * Time.deltaTime));
        filledImageStamina.fillAmount = player.GetCurrentStamina() / player.GetMaxStamina();
    }
    void TakeDamage()
    {
        if (character.GetEnergy().GetMax() > 0)
            filledImageEnergy.fillAmount = character.GetEnergy().GetCurrent() / character.GetEnergy().GetMax();
        if (character.GetArmor().GetMax() > 0)
            filledImageArmor.fillAmount = character.GetArmor().GetCurrent() / character.GetArmor().GetMax();
    }
    void Shoot(float maxCoolDown, bool reloading)
    {
        if (reloading)
        {
            StartCoroutine(Reloading(maxCoolDown));
        }
    }
    void Death()
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
    public void Pause()
    {
        if (player.playerStatus == PlayerController.PlayerStatus.Game || player.playerStatus == PlayerController.PlayerStatus.Inventory)
        {
            fromInventory = (player.playerStatus == PlayerController.PlayerStatus.Inventory);
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
        Time.timeScale = 0;
        player.playerStatus = PlayerController.PlayerStatus.Fading;
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
        player.playerStatus = PlayerController.PlayerStatus.Pause;
        player.AvailableCursor(true);
        menuActual = CanvasGroupList.Pause;
    }
    IEnumerator PauseDisabling()
    {
        player.playerStatus = PlayerController.PlayerStatus.Fading;
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
        player.playerStatus = PlayerController.PlayerStatus.Game;
        Time.timeScale = 1;
        player.AvailableCursor(fromInventory);
        menuActual = CanvasGroupList.GamePlay;
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
}