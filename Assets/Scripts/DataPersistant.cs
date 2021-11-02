using System;
using UnityEngine;
public class DataPersistant : MonoBehaviourSingleton<DataPersistant>
{
    public UiOptions uiOptions;
    public GameSettings gameSettings;

    [HideInInspector] public PlayerController playerController;
    void Start()
    {
        uiOptions.SetAllValues();
    }
    private void OnDestroy()
    {
        PlayerPrefs.DeleteAll();    // Todo: Ojo con esto. Eliminar al utilizar PlayerPrefabs en el Player -> Utilizar Json para el inventario.
    }
    public void SetSensitives()
    {
        if (playerController)
        {
            Vector2 sensitives = new Vector2(gameSettings.controls.GetSensitiveHorizontal(), gameSettings.controls.GetSensitiveVertical());
            playerController.SetSensitive(sensitives);
        }
    }

    public void LoadScene(PlayerController playerC) => playerController = playerC;
    public void InitialSettingsGame(UiOptions uiOption)
    {
        uiOptions = uiOption;
        uiOptions.onChangeControl += ChangeControls;
        uiOptions.onChangeGeneral += ChangeGenerals;
        uiOptions.onChangeSound += ChangeSounds;
    }
    public void ChangeControls(Controls control)
    {
        gameSettings.controls.SetSensitiveHorizontal(control.GetSensitiveHorizontal());
        gameSettings.controls.SetSensitiveVertical(control.GetSensitiveVertical());
    }
    public void ChangeGenerals(General general) // Todo: FALTA CARGAR DEMAS VARIABLES
    {
        //gameSettings.general.SetQualityText(general.GetQualityLevel());
        gameSettings.general.SetFullScreenMode(general.GetFullScreenMode());
        gameSettings.general.SetVsyncMode(general.GetVsyncMode());
    }
    public void ChangeSounds(Sounds sounds)
    {
        gameSettings.sounds.SetVolumeGeneral(sounds.GetVolumeGeneral());
        gameSettings.sounds.SetVolumeMusic(sounds.GetVolumeMusic());
        gameSettings.sounds.SetVolumeEffects(sounds.GetVolumeEffect());
    }
}


[Serializable] public class GameSettings
{
    public Controls controls;
    public General general;
    public Sounds sounds;
}


[Serializable] public class Controls
{
    public Action changeSensitivePlayer;
    [SerializeField] private float sensitiveHor;
    [SerializeField] private float sensitiveVer;

    public Vector2 GetSensitives() => new Vector2(sensitiveHor, sensitiveVer);
    public float GetSensitiveHorizontal() => sensitiveHor;
    public float GetSensitiveVertical() => sensitiveVer;

    public void SetSensitives(Vector2 sensitive)
    {
        sensitiveHor = sensitive.x;
        sensitiveVer = sensitive.y;
        changeSensitivePlayer?.Invoke();
        DataPersistant.Get().SetSensitives();
    }
    public void SetSensitiveHorizontal(float value)
    {
        sensitiveHor = value;
        changeSensitivePlayer?.Invoke();
        DataPersistant.Get().SetSensitives();
    }
    public void SetSensitiveVertical(float value)
    {
        sensitiveVer = value;
        changeSensitivePlayer?.Invoke();
        DataPersistant.Get().SetSensitives();
    }
}


[Serializable] public class General
{
    [SerializeField] private string[] qualityLevelNames = new string[] {"Low", "Normal", "High", "Ultra"};
    [SerializeField] private Vector2Int[] resolutions;
    [SerializeField] private int currentResolution;

    [SerializeField] private int qualityLevel = 3;
    [SerializeField] private bool fullScreen = true;
    [SerializeField] private bool vSync = true;

    public int GetQualityLevel()
    {
        qualityLevel = QualitySettings.GetQualityLevel();
        return qualityLevel;
    }
    public void IncreaseLevel()
    {
        qualityLevel++;
        if (qualityLevel > qualityLevelNames.Length - 1)
        {
            qualityLevel = 0;
            QualitySettings.SetQualityLevel(qualityLevel);
        }
        else
        {
            QualitySettings.IncreaseLevel();
            qualityLevel = QualitySettings.GetQualityLevel();
        }
    }
    public void DecreaseLevel()
    {
        qualityLevel--;
        if (qualityLevel < 0)
        {
            qualityLevel = qualityLevelNames.Length - 1;
            QualitySettings.SetQualityLevel(qualityLevel);
        }
        else
        {
            QualitySettings.DecreaseLevel();
            qualityLevel = QualitySettings.GetQualityLevel();
        }
    }
    public bool GetFullScreenMode() => fullScreen;
    public bool GetVsyncMode() => vSync;
    public int GetCurrentResolution() => currentResolution;
    public int GetMaxQuality() => qualityLevelNames.Length;
    public string GetNameQuality(int index) => qualityLevelNames[index];
    public string GetNameCurrentQuality() => qualityLevelNames[qualityLevel];

    public void SetAllConfigurations(int quality, bool fullScreen, int currentResolution, Vector2Int resolution, int vSync)
    {
        this.qualityLevel = quality;
        this.fullScreen = fullScreen;
        this.currentResolution = currentResolution;

        QualitySettings.SetQualityLevel(quality);
        Screen.SetResolution(resolution.x, resolution.y, fullScreen);
        QualitySettings.vSyncCount = vSync;
    }
    public void SetQualityLevel(int value)
    {
        qualityLevel = value;
        QualitySettings.IncreaseLevel();
    }
    public void SetFullScreenMode(bool value)
    {
        fullScreen = value;
        Screen.fullScreenMode = fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        string textFullScreen = fullScreen ? "Full" : "Windowed";
        Debug.Log("FullScreenMode en: " + textFullScreen);
    }
    public void SetResolution(int resolution)
    {
        currentResolution = resolution;
        int width = resolutions[currentResolution].x;
        int height = resolutions[currentResolution].y;
        Screen.SetResolution(width, height, fullScreen);

        Debug.Log("Resolucion en: " + width + " x " + height);
    }
    public void SetVsyncMode(bool modeVsync)
    {
        vSync = modeVsync;
        QualitySettings.vSyncCount = vSync ? 1 : 0;
        Debug.Log("vSyncCount en: " + QualitySettings.vSyncCount);
    }
    public void AlternateVsyncMode()
    {
        vSync = !vSync;
        QualitySettings.vSyncCount = vSync ? 1 : 0;
        Debug.Log("vSyncCount en: " + QualitySettings.vSyncCount);
    }
    public void AlternateFullScreenMode()
    {
        fullScreen = !fullScreen;
        Screen.fullScreenMode = fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        string textFullScreen = fullScreen ? "Full" : "Windowed";
        Debug.Log("FullScreenMode en: " + textFullScreen);
    }
}


[Serializable] public class Sounds
{
    [SerializeField] private float general;
    [SerializeField] private float music;
    [SerializeField] private float effects;

    public float GetVolumeGeneral() => general;
    public float GetVolumeMusic() => music;
    public float GetVolumeEffect() => effects;

    public void SetAllVolumes(float volGeneral, float volMusic, float volEffect)
    {
        SetVolumeGeneral(volGeneral);
        SetVolumeMusic(volMusic);
        SetVolumeEffects(volEffect);
    }
    public void SetVolumeGeneral(float value)
    {
        general = value;
        // Evento a Wwise
    }
    public void SetVolumeMusic(float value)
    {
        music = value;
        // Evento a Wwise
    }
    public void SetVolumeEffects(float value)
    {
        effects = value;
        // Evento a Wwise
    }
}