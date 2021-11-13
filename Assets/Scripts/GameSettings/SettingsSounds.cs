using UnityEngine;
public class SettingsSounds
{
    public bool soundGeneralOn = true;
    public bool soundMusicOn = true;
    public bool soundEffectOn = true;

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
    }
    public void SetVolumeMusic(float value)
    {
        music = value;
    }
    public void SetVolumeEffects(float value)
    {
        effects = value;
    }
}