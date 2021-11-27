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
        Beach,
        Boss
    }

    [HideInInspector] public string[] test = {"None", "Field", "Forest", "Beach", "Boss"};
    public List<Enviroment> enviroments;
}