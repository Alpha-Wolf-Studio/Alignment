using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NaveCapsula : MonoBehaviour
{
    private Camera cam;
    private float onTime;
    [SerializeField] private float transitionTime;
    [SerializeField] private float rotationTime;
    private Vector3 camInitialPos;
    private Vector3 navePos;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }
    void Start()
    {
        Time.timeScale = 0.5f;
        camInitialPos = cam.transform.position;
        navePos = transform.position;
    }
    public void StartAnim()
    {
        anim.SetTrigger("Launch");
    }
    public void SetCameraChild()
    {
        cam.transform.SetParent(gameObject.transform);
        anim.enabled = false;
    }
    public void StartCamera()
    {
        StartCoroutine(MoveCam());
    }
    IEnumerator MoveCam()
    {
        while (onTime < transitionTime)
        {
            //Debug.Log("Moviendo");
            navePos = transform.position;
            onTime += Time.unscaledDeltaTime;
            cam.transform.position = Vector3.Slerp(camInitialPos, navePos, onTime / transitionTime);
            yield return null;
        }
        onTime = 0;
        StartCoroutine(RotateCam());
    }
    IEnumerator RotateCam()
    {
        Quaternion rotation = cam.transform.localRotation;
        while (onTime < rotationTime)
        {
            //Debug.Log("Rotando");
            onTime += Time.unscaledDeltaTime;
            cam.transform.localRotation = Quaternion.Lerp(rotation, Quaternion.identity, onTime / rotationTime);
            yield return null;
        }
        anim.enabled = true;
        onTime = 0;
    }
    public void EndAnimation()
    {
        SceneManager.LoadScene("Game");
    }
}