using UnityEngine;
public class SettingsSounds
{
    private float general = 1;
    private float music = 1;
    private float effects = 1;

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