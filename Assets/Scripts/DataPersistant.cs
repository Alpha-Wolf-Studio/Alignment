using UnityEngine;
public class DataPersistant : MonoBehaviourSingleton<DataPersistant>
{
    public UiOptions uiOptions;
    public GameSettings gameSettings;

    [HideInInspector] public PlayerController playerController;
    void Start()
    {
        gameSettings = new GameSettings {controls = new SettingsControls(), general = new SettingsGeneral(), sounds = new SettingsSounds()};
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
    public void ChangeControls(SettingsControls control)
    {
        gameSettings.controls.SetSensitiveHorizontal(control.GetSensitiveHorizontal());
        gameSettings.controls.SetSensitiveVertical(control.GetSensitiveVertical());
    }
    public void ChangeGenerals(SettingsGeneral general) // Todo: FALTA CARGAR DEMAS VARIABLES
    {
        //gameSettings.general.SetQualityText(general.GetQualityLevel());
        gameSettings.general.SetFullScreenMode(general.GetFullScreenMode());
        gameSettings.general.SetVsyncMode(general.GetVsyncMode());
    }
    public void ChangeSounds(SettingsSounds sounds)
    {
        gameSettings.sounds.SetVolumeGeneral(sounds.GetVolumeGeneral());
        gameSettings.sounds.SetVolumeMusic(sounds.GetVolumeMusic());
        gameSettings.sounds.SetVolumeEffects(sounds.GetVolumeEffect());
    }
}