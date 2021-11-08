using System;
using UnityEngine;
public class SettingsControls
{
    public Action changeSensitivePlayer;
    [SerializeField] private float sensitiveHor = 2;
    [SerializeField] private float sensitiveVer = 2;

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