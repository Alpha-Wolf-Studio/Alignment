using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiOptions : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Image[] buttons;
    [SerializeField] private Image[] panels;

    [Header("Sprites")] 
    [SerializeField] private Sprite[] spritesControls;
    [SerializeField] private Sprite[] spritesSettings;
    [SerializeField] private Sprite[] spritesSounds;

    [Header("Other")]
    private int qualityLevel;
    private bool fullScreen = true;
    [SerializeField] private Vector2Int[] resolutions;
    private int currentResolution;

    //---------- LeftPanels ----------
    public void ChangePanelToControls()
    {
        OffAllSprites();
        buttons[0].sprite = spritesControls[0];
        panels[0].gameObject.SetActive(true);
    }

    public void ChangePanelToSettings()
    {
        OffAllSprites();
        buttons[1].sprite = spritesSettings[0];
        panels[1].gameObject.SetActive(true);
    }

    public void ChangePanelToSounds()
    {
        OffAllSprites();
        buttons[2].sprite = spritesSounds[0];
        panels[2].gameObject.SetActive(true);
    }

    void OffAllSprites()
    {
        buttons[0].sprite = spritesControls[1];
        buttons[1].sprite = spritesSettings[1];
        buttons[2].sprite = spritesSounds[1];

        for (int i = 0; i < panels.Length; i++)
            panels[i].gameObject.SetActive(false);
    }

    //---------- Controls ----------
    public void ControlChangeSensitiveHor()
    {

    }
    public void ControlChangeSensitiveVer()
    {

    }

    //---------- General -----------
    public void GeneralChangeQualityGame()
    {
        QualitySettings.SetQualityLevel(qualityLevel);
    }
    public void GeneralChangeWindowedMode()
    {
        fullScreen = !fullScreen;
        Screen.fullScreenMode = fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Debug.Log("fullScreenMode en: " + Screen.fullScreenMode);
    }
    public void GeneralChangeResolution()
    {
        int width = resolutions[currentResolution].x;
        int height = resolutions[currentResolution].y;

        Screen.SetResolution(width, height, fullScreen);
    }
    public void GeneralChangeVsync()
    {
        QualitySettings.vSyncCount = QualitySettings.vSyncCount == 1 ? 0 : 1;
        Debug.Log("vSyncCount en: " + QualitySettings.vSyncCount);
    }

    //---------- Sounds ------------
    public void SoundToggleGeneral()
    {

    }
    public void SoundToggleMusic()
    {

    }
    public void SoundToggleEffect()
    {

    }
    public void SoundChangeGeneral()
    {

    }
    public void SoundChangeMusic()
    {

    }
    public void SoundChangeEffect()
    {

    }
}