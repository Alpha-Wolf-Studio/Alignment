using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Image filledImageCoolDown;
    public Image filledImageEnergy;
    public Image filledImageArmor;
    public Color CoolDownReloading = Color.red;
    public Character character;
    public PlayerController player;
    public RectTransform sightHud;
    private float speed = 0.1f;
    private float onTimeCD;
    private float onTimeFadePause;
    private float fadePause = 1;

    public enum CanvasGroupList { GamePlay, Pause, Options }
    public CanvasGroupList menuActual;
    public List<CanvasGroup> canvasGroup = new List<CanvasGroup>();

    void Start()
    {
        player.playerStatus = PlayerController.PlayerStatus.Inization;
        character.OnUpdateStats += TakeDamage;
        character.OnDeath += Death;
        player.onShoot+= Shoot;
        player.OnPause+= Pause;
    }
    void Update()
    {
        sightHud.Rotate(Vector3.forward * (speed * Time.deltaTime));
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
    public void Pause()
    {
        StartCoroutine(player.playerStatus == PlayerController.PlayerStatus.Game ? PauseEnabling() : PauseDisabling());
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
        menuActual = CanvasGroupList.Pause;
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

            yield return null;
        }

        canvasGroup[(int) CanvasGroupList.Pause].interactable = true;
        canvasGroup[(int) CanvasGroupList.Pause].blocksRaycasts = true;

        onTimeFadePause = 0;
        player.playerStatus = PlayerController.PlayerStatus.Pause;
        player.AvailableCursor(true);
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

            yield return null;
        }

        canvasGroup[(int)CanvasGroupList.Pause].alpha = 0;
        canvasGroup[(int)CanvasGroupList.Options].alpha = 0;
        EnableCanvasGroup(canvasGroup[(int) CanvasGroupList.GamePlay], true);
        onTimeFadePause = 0;
        player.playerStatus = PlayerController.PlayerStatus.Game;
        Time.timeScale = 1;
        player.AvailableCursor(false);
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