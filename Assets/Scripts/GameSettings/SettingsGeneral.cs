using UnityEngine;
public class SettingsGeneral
{
    private string[] qualityLevelNames = new string[] { "Low", "Normal", "High", "Ultra" };
    private Vector2Int[] resolutions = new Vector2Int[]
    {
        new Vector2Int(2560, 1080),
        new Vector2Int(1920, 1080),
        new Vector2Int(1280, 720),
        new Vector2Int(960, 540),
        new Vector2Int(640, 360)
    };
    private int currentResolution = 1;

    private int qualityLevel = 3;
    private bool fullScreen = true;
    private bool vSync = true;

    public void SetInitialValues()
    {
        SetResolution(1);
    }
    public bool GetFullScreenMode() => fullScreen;
    public bool GetVsyncMode() => vSync;
    public int GetCurrentResolution() => currentResolution;
    public Vector2 GetCurrentResolutionVector2() => resolutions[currentResolution];
    public int GetMaxQuality() => qualityLevelNames.Length;
    public string GetNameQuality(int index) => qualityLevelNames[index];
    public string GetNameCurrentQuality() => qualityLevelNames[qualityLevel];
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