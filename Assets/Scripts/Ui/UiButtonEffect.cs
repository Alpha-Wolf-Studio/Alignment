using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("RayCast Collision:")]
    [Tooltip("Chequea Alphas en el raycast. Modificar el Read/Write Enabled en la imagen si éste es true.")]
    [SerializeField] private bool modifyHitBox;
    [SerializeField] private float alphaRayCast = 0.1f;
    
    [Header("Effect Scale:")]
    [SerializeField] private float scaleSpeed= 3;
    [SerializeField] private float scaleLimit = 1.2f;
    private bool increment = false;
    private Vector3 initialScale;
    private Vector3 scale;

    private void Awake()
    {
        increment = false;
        initialScale = transform.localScale;
        if (modifyHitBox)
            GetComponent<Image>().alphaHitTestMinimumThreshold = alphaRayCast;
    }
    private void OnEnable()
    {
        transform.localScale = initialScale;
        increment = false;
    }
    private void Update()
    {
        ChangeScale();
    }
    public void OnMouseEnterButton()
    {
        increment = true;
        if (Sfx.Get().GetEnable(Sfx.ListSfx.UiButtonEnter))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiButtonEnter), gameObject);
    }
    public void OnMouseExitButton()
    {
        increment = false;
        if (Sfx.Get().GetEnable(Sfx.ListSfx.UiButtonExit))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiButtonExit), gameObject);
    }
    private void ChangeScale()
    {
        float timeStep = scaleSpeed * Time.unscaledDeltaTime;
        scale = transform.localScale;
        if (increment)
        {
            if (transform.localScale.x < scaleLimit)
            {
                scale = new Vector3(scale.x + timeStep, scale.y + timeStep, scale.z + timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(scaleLimit, scaleLimit, scaleLimit);
            }
        }
        else
        {
            if (transform.localScale.x > initialScale.x)
            {
                scale = new Vector3(scale.x - timeStep, scale.y - timeStep, scale.z - timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterButton();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitButton();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Sfx.Get().GetEnable(Sfx.ListSfx.UiClickButton))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiClickButton), gameObject);
    }
}