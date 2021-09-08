using UnityEngine;

[CreateAssetMenu(fileName = "Kill Dinosaur Task", menuName = "Quest/Task/Kill Dinosaur")]
public class KillDinosTask : Task
{
    public DinoClass dinosaurToKill = DinoClass.Raptor;
    public int amount = 1;
}
