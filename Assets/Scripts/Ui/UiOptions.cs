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
    [SerializeField] private TextMeshProUGUI qualityLevelText;
    [SerializeField] private TMP_Dropdown dropdownResolutions;
    private int qualityLevel = 3;
    [SerializeField] private Vector2Int[] resolutions;

    private string[] qualityLevelNames = new string[] {"Low", "Normal", "High", "Ultra"};
    private bool fullScreen = true;
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
    public void ControlChangeSensitiveHor(float value)
    {
        Debug.Log("Sensivilidad Horizontal Cambiada a: " + value);

    }
    public void ControlChangeSensitiveVer(float value)
    {
        Debug.Log("Sensivilidad Vertical Cambiada a: " + value);

    }

    //---------- General -----------
    public void GeneralChangeQualityGame(bool isNext)
    {
        if (isNext)
        {
            qualityLevel++;
            if (qualityLevel >= qualityLevelNames.Length)
                qualityLevel = 0;
        }
        else
        {
            qualityLevel--;
            if (qualityLevel < 0)
                qualityLevel = qualityLevelNames.Length - 1;
        }

        Debug.Log("Quality Level Cambiado a " + qualityLevel + ": " + qualityLevelNames[qualityLevel]);
        qualityLevelText.text = qualityLevelNames[qualityLevel];
        QualitySettings.SetQualityLevel(qualityLevel);
    }
    public void GeneralChangeWindowedMode()
    {
        fullScreen = !fullScreen;
        Screen.fullScreenMode = fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Debug.Log("FullScreenMode en: " + Screen.fullScreenMode);
    }
    public void GeneralChangeResolution()
    {
        currentResolution = dropdownResolutions.value;
        int width = resolutions[currentResolution].x;
        int height = resolutions[currentResolution].y;
        Debug.Log("Resolucion en: " + width + " x " + height);

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
    public void SoundChangeGeneral(float value)
    {
        Debug.Log("General Cambiado a: " + value);

    }
    public void SoundChangeMusic(float value)
    {
        Debug.Log("Musicas Cambiada a: " + value);

    }
    public void SoundChangeEffect(float value)
    {
        Debug.Log("Efectos Cambiado a: " + value);

    }
}