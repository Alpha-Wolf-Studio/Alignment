using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiOptions : MonoBehaviour
{
    public bool isGamePlay;
    public Action<SettingsControls> onChangeControl;
    public Action<SettingsGeneral> onChangeGeneral;
    public Action<SettingsSounds> onChangeSound;

    [Header("Components")] 
    [SerializeField] private Image[] buttons;
    [SerializeField] private Image[] panels;

    [Header("Sprites")] 
    [SerializeField] private Sprite[] spritesControls;
    [SerializeField] private Sprite[] spritesSettings;
    [SerializeField] private Sprite[] spritesSounds;

    [Header("Controls")]
    [SerializeField] private Slider sliderSensitiveHorizontal;
    [SerializeField] private Slider sliderSensitiveVertical;

    [Header("General")]
    [SerializeField] private TextMeshProUGUI qualityLevelText;
    [SerializeField] private Toggle windowed;
    [SerializeField] private TMP_Dropdown dropdownResolutions;
    [SerializeField] private Toggle vSync;

    [Header("Sounds")]
    [SerializeField] private Slider sliderVolumeGeneral;
    [SerializeField] private Slider sliderVolumeMusic;
    [SerializeField] private Slider sliderVolumeEffects;
    
    private void Start()
    {
        DataPersistant.Get().InitialSettingsGame(this);
        UpdateAllValues();
    }
    public void UpdateAllValues()
    {
        SettingsControls controls = DataPersistant.Get().gameSettings.controls;
        SettingsGeneral general = DataPersistant.Get().gameSettings.general;
        SettingsSounds sounds = DataPersistant.Get().gameSettings.sounds;

        // Controls:
        sliderSensitiveHorizontal.value = controls.GetSensitiveHorizontal();
        sliderSensitiveVertical.value = controls.GetSensitiveVertical();

        // General:
        UpdateTextQuality();
        windowed.isOn = general.GetFullScreenMode();
        dropdownResolutions.value = general.GetCurrentResolution();
        vSync.isOn = general.GetVsyncMode();

        // Sounds:
        sliderVolumeGeneral.value = sounds.GetVolumeGeneral();
        sliderVolumeMusic.value = sounds.GetVolumeMusic();
        sliderVolumeEffects.value = sounds.GetVolumeEffect();

        if (isGamePlay)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            DataPersistant.Get().LoadScene(playerController);
        }
    }
    public void SetAllValues()
    {
        // Controls:
        Vector2 sensitives = new Vector2(sliderSensitiveHorizontal.value, sliderSensitiveVertical.value);
        DataPersistant.Get().gameSettings.controls.SetSensitives(sensitives);

        // General:
        int quality = QualitySettings.GetQualityLevel();
        bool fullScreen = Screen.fullScreen;
        int currentResolution = 0;
        Vector2Int resolution = new Vector2Int(Screen.currentResolution.width, Screen.currentResolution.height);
        int vSync = QualitySettings.vSyncCount;
        DataPersistant.Get().gameSettings.general.SetAllConfigurations(quality, fullScreen, currentResolution, resolution, vSync);

        // Sounds:
        DataPersistant.Get().gameSettings.sounds.SetAllVolumes(sliderVolumeGeneral.value, sliderVolumeMusic.value, sliderVolumeEffects.value);
    }

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
    public void ControlChangeSensitiveHor(float value)
    {
        DataPersistant.Get().gameSettings.controls.SetSensitiveHorizontal(value);
        Debug.Log("Sensivilidad Horizontal Cambiada a: " + value);
    }
    public void ControlChangeSensitiveVer(float value)
    {
        DataPersistant.Get().gameSettings.controls.SetSensitiveVertical(value);
        Debug.Log("Sensivilidad Vertical Cambiada a: " + value);
    }

    //---------- General -----------
    public void GeneralChangeQualityGame(bool isNext)
    {
        if (isNext)
            DataPersistant.Get().gameSettings.general.IncreaseLevel();
        else
            DataPersistant.Get().gameSettings.general.DecreaseLevel();

        UpdateTextQuality();
    }
    void UpdateTextQuality()
    {
        string qualityName = DataPersistant.Get().gameSettings.general.GetNameCurrentQuality();
        qualityLevelText.text = qualityName;
    }                         
    public void GeneralChangeWindowedMode()
    {
        DataPersistant.Get().gameSettings.general.SetFullScreenMode(windowed.isOn);
    }          
    public void GeneralChangeResolution()
    {
        DataPersistant.Get().gameSettings.general.SetResolution(dropdownResolutions.value);
    }            
    public void GeneralChangeVsync()
    {
        DataPersistant.Get().gameSettings.general.AlternateVsyncMode();
    }                 

    //---------- Sounds ------------
    public void SoundToggleGeneral()
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeGeneral(0);
        sliderVolumeGeneral.value = 0;
    }
    public void SoundToggleMusic()
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeMusic(0);
        sliderVolumeMusic.value = 0;
    }
    public void SoundToggleEffect()
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeEffects(0);
        sliderVolumeEffects.value = 0;
    }
    public void SoundChangeGeneral(float value)
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeGeneral(value);
        Debug.Log("General Cambiado a: " + value);
    }
    public void SoundChangeMusic(float value)
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeMusic(value);
        Debug.Log("Musicas Cambiada a: " + value);
    }
    public void SoundChangeEffect(float value)
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeEffects(value);
        Debug.Log("Efectos Cambiado a: " + value);
    }
}