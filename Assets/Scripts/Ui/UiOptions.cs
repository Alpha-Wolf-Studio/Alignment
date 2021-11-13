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
    [SerializeField] private Image imgGeneral;
    [SerializeField] private Image imgMusic;
    [SerializeField] private Image imgEffect;
    [SerializeField] private Color colorOffSound;

    private void Start()
    {
        DataPersistant.Get().InitialSettingsGame(this);
        UpdateAllValues();
    }

    private SettingsControls controls;
    private SettingsGeneral general;
    private SettingsSounds sounds;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            controls = DataPersistant.Get().gameSettings.controls;
            general = DataPersistant.Get().gameSettings.general;
            sounds = DataPersistant.Get().gameSettings.sounds;

            Debug.Log("General: " + sounds.soundGeneralOn + " " + sounds.GetVolumeGeneral());
            Debug.Log("Music: " + sounds.soundMusicOn + " " + sounds.GetVolumeMusic());
            Debug.Log("Effect: " + sounds.soundEffectOn + " " + sounds.GetVolumeEffect());
        }
    }

    public void UpdateAllValues()
    {
        controls = DataPersistant.Get().gameSettings.controls;
        general = DataPersistant.Get().gameSettings.general;
        sounds = DataPersistant.Get().gameSettings.sounds;

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

        Debug.Log("General: " + sounds.soundGeneralOn + " " + sounds.GetVolumeGeneral());
        Debug.Log("Music: " + sounds.soundMusicOn + " " + sounds.GetVolumeMusic());
        Debug.Log("Effect: " + sounds.soundEffectOn + " " + sounds.GetVolumeEffect());

        if (isGamePlay)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            DataPersistant.Get().LoadScene(playerController);
        }
        
        ChangeColorButton(imgGeneral, DataPersistant.Get().gameSettings.sounds.soundGeneralOn ? Color.white : colorOffSound);
        ChangeColorButton(imgMusic, DataPersistant.Get().gameSettings.sounds.soundMusicOn ? Color.white : colorOffSound);
        ChangeColorButton(imgEffect, DataPersistant.Get().gameSettings.sounds.soundEffectOn ? Color.white : colorOffSound);
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
        DataPersistant.Get().gameSettings.sounds.soundGeneralOn = !DataPersistant.Get().gameSettings.sounds.soundGeneralOn;
        float volume = DataPersistant.Get().gameSettings.sounds.soundGeneralOn ? DataPersistant.Get().gameSettings.sounds.GetVolumeGeneral() : 0;
        
        AkSoundEngine.SetRTPCValue("VolGeneralValue", volume);
        ChangeColorButton(imgGeneral, DataPersistant.Get().gameSettings.sounds.soundGeneralOn ? Color.white : colorOffSound);
    }
    public void SoundToggleMusic()
    {
        DataPersistant.Get().gameSettings.sounds.soundMusicOn = !DataPersistant.Get().gameSettings.sounds.soundMusicOn;
        float volume = DataPersistant.Get().gameSettings.sounds.soundGeneralOn ? DataPersistant.Get().gameSettings.sounds.GetVolumeGeneral() : 0;
        
        AkSoundEngine.SetRTPCValue("VolMusicValue", volume);
        ChangeColorButton(imgMusic, DataPersistant.Get().gameSettings.sounds.soundMusicOn ? Color.white : colorOffSound);
    }
    public void SoundToggleEffect()
    {
        DataPersistant.Get().gameSettings.sounds.soundEffectOn = !DataPersistant.Get().gameSettings.sounds.soundEffectOn;
        float volume = DataPersistant.Get().gameSettings.sounds.soundGeneralOn ? DataPersistant.Get().gameSettings.sounds.GetVolumeGeneral() : 0;
        
        AkSoundEngine.SetRTPCValue("VolEffectValue", volume);
        ChangeColorButton(imgEffect, DataPersistant.Get().gameSettings.sounds.soundEffectOn ? Color.white : colorOffSound);
    }
    public void SoundChangeGeneral(float value)
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeGeneral(value);
        AkSoundEngine.SetRTPCValue("VolGeneralValue", value);
        Debug.Log("VolGeneralValue Cambiado a: " + value);
        ChangeColorButton(imgGeneral, Color.white);
        DataPersistant.Get().gameSettings.sounds.soundGeneralOn = true;
    }
    public void SoundChangeMusic(float value)
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeMusic(value);
        AkSoundEngine.SetRTPCValue("VolMusicValue", value);
        Debug.Log("VolMusicValue Cambiada a: " + value);
        ChangeColorButton(imgMusic, Color.white);
        DataPersistant.Get().gameSettings.sounds.soundMusicOn = true;
    }
    public void SoundChangeEffect(float value)
    {
        DataPersistant.Get().gameSettings.sounds.SetVolumeEffects(value);
        AkSoundEngine.SetRTPCValue("VolEffectValue", value);
        Debug.Log("VolEffectValue Cambiado a: " + value);
        ChangeColorButton(imgEffect, Color.white);
        DataPersistant.Get().gameSettings.sounds.soundEffectOn = true;
    }
    void ChangeColorButton(Image btn, Color color)
    {
        btn.color = color;
    }
}