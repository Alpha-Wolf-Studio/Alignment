using UnityEngine;
using UnityEngine.EventSystems;
public class UiButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private float scaleMultiply = 3;
    [SerializeField] private float limit = 1.2f;
    private bool increment = false;
    private Vector3 initialScale;
    private Vector3 scale;

    private void Awake()
    {
        increment = false;
        initialScale = transform.localScale;
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
        float timeStep = scaleMultiply * Time.unscaledDeltaTime;
        scale = transform.localScale;
        if (increment)
        {
            if (transform.localScale.x < limit)
            {
                scale = new Vector3(scale.x + timeStep, scale.y + timeStep, scale.z + timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(limit, limit, limit);
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