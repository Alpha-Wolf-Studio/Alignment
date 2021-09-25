using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest/Quest")]
public class Quest : ScriptableObject
{
    public string questTitle = "";
    public List<SubQuest> tasks = new List<SubQuest>();
}
