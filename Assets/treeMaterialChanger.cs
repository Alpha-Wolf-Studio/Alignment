using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class treeMaterialChanger : MonoBehaviour
{

    [SerializeField] List<Material> possibleMaterials = null;
    [SerializeField] bool reroll = false;

    void Update()
    {
        if (reroll) 
        {
            var renderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var rend in renderers)
            {
                rend.material = possibleMaterials[Random.Range(0, possibleMaterials.Count)];
            }
            reroll = false;
        }
    }
}
