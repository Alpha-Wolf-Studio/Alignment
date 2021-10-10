using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UiItemInventory : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UiInventory uiInv;
    [SerializeField] private int indexList;
    [SerializeField] private int id;

    public Image myImage;
    public TextMeshProUGUI textAmount;

    private RectTransform rectTransform;
    public int GetID() => id;
    public int GetIndex() => indexList;

    private void Awake()
    {
        uiInv = FindObjectOfType<UiInventory>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        uiInv.onRefreshAllButtonsEvent += Refresh;
    }
    public void SetButton(int indexList, int id)
    {
        this.indexList = indexList;
        this.id = id;
        
        myImage.sprite = ItemManager.GetInstance().GetItemFromID(id).icon;
        myImage.color = (id > 0) ? Color.white : Color.clear;
        if (ItemManager.GetInstance().GetItemFromID(id).maxStack > 1)
        {
            textAmount.text = uiInv.inventory.GetSlot(indexList).amount.ToString();
        }
        textAmount.gameObject.SetActive(ItemManager.GetInstance().GetItemFromID(id).maxStack > 1);
    }
    public void Refresh()
    {
        id = uiInv.inventory.GetID(indexList);
        SetButton(indexList, id);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Enter");
        if (uiInv.picked)
        {
            //Debug.Log("OnPointerUp: " + gameObject.name, gameObject);
            uiInv.slotDrop = this;
            uiInv.SwapButtonsIDs();
            uiInv.picked = false;
        }

        if (id > 0)
        {
            uiInv.toolTip.gameObject.SetActive(true);
            uiInv.MouseEnterOver(rectTransform);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown: " + gameObject.name, gameObject);
        if (id > 0)
        {
            if (Input.GetMouseButton(0))
            {
                uiInv.slotAux.transform.position = Input.mousePosition;
                uiInv.slotAux.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
                uiInv.slotAux.gameObject.SetActive(true);
                uiInv.slotAux.transform.GetChild(0).GetComponent<Image>().sprite = myImage.sprite;

                uiInv.toolTip.gameObject.SetActive(false);

                uiInv.picked = true;
                uiInv.slotPick = this;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                uiInv.inventory.Divide(indexList); // Ya que dividir es Void, me podría devolver la ubicación en la lista donde se metió la otra mitad así en vez de refrescar todos los botones, solo refresco el actual y donde fue a parar
                uiInv.RefreshAllButtons();
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        uiInv.slotAux.transform.position = Input.mousePosition;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        uiInv.slotAux.transform.position = Input.mousePosition;
        uiInv.slotAux.gameObject.SetActive(false);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        uiInv.toolTip.gameObject.SetActive(false);
    }
}