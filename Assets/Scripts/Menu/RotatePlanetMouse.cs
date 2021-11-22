using UnityEngine;
public class RotatePlanetMouse : MonoBehaviour
{
    [SerializeField] private UIMenuManager uiMenuManager;
    [SerializeField] private float sensitivity = 5;
    private Vector3 rotation = Vector3.zero;
    private Vector3 pos;

    private void Start()
    {
        AkSoundEngine.PostEvent(AK.EVENTS.PLAYMENUMUSIC, gameObject);
        pos = transform.position;
    }
    private void OnMouseDrag()
    {
        if (uiMenuManager.menuActual == UIMenuManager.Menues.Play)
            return;

        Vector2 axisRot = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * sensitivity;

        transform.RotateAround(pos, Vector3.down, axisRot.x);
        transform.RotateAround(pos, Vector3.right, axisRot.y);

    }
    private void Reset()
    {
        sensitivity = 5;
        rotation = Vector3.zero;
    }
}