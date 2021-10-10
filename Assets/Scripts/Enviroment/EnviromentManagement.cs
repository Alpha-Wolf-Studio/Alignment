using System.Collections.Generic;
using UnityEngine;
public class EnviromentManagement : MonoBehaviour
{
    public LayerMask player;
    public enum Enviroments
    {
        None,
        Field,
        Forest,
        Beach
    }

    [HideInInspector] public string[] test = {"None", "Field", "Forest", "Beach"};
    public List<Enviroment> enviroments;
}