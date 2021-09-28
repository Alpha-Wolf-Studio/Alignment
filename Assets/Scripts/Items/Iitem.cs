using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iitem : MonoBehaviour
{
    [SerializeField] ItemComponent realComponent;
    public ItemComponent GetItemComponent() => realComponent;

    private void Awake()
    {
        realComponent.itemInterface = this;
    }
}
