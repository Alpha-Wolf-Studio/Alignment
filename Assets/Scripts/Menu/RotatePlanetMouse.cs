using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotatePlanetMouse : MonoBehaviour
{
    private Transform planet;
    private Rigidbody rbPlanet;
    private Vector3 mousePos;
    public LayerMask layerEnviroment;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Terra ON");
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            mousePos = Input.mousePosition;
            if (Physics.Raycast(Camera.main.transform.position, mousePos, 999, layerEnviroment))
            {
                Debug.Log("Terra OFF");
            }
            Debug.Log("Terra OFF2222");
        }
    }
}