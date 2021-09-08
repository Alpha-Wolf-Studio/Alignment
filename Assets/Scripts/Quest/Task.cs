using UnityEngine;
public class Task : ScriptableObject
{
    bool completed = false;
    public bool IsFinished() => completed;
    public void Finish() => completed = true;
}
