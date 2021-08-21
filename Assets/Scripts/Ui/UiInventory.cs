using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    public enum InventaryStatus { Open, Close, Opening, Closeing }
    public InventaryStatus inventaryStatus = InventaryStatus.Close;

    public GameObject player;
    [HideInInspector] public Inventory inventory;
    private PlayerController playerController;

    public Image defaultSprite;
    // Agarrar un evento para agarrar cuando el Inventario esté totalmente cargado.
    public Action onRefreshAllButtonsEvent;
    public Image slotAux;
    public Button prefabItemInventory;
    public GameObject item;
    public GameObject toolTip;
    public Button buttonSortName;

    private float onTime;
    private float onTime2;
    public float panelOpacityOn = 1.0f;
    public float inventoryOpenDuration = 1.0f;
    private CanvasGroup panelGral;
    public RectMask2D rmPanelInventory;
    public RectMask2D rmPanelCrafting;
    public RectMask2D rmPanelCharacter;
    private RectTransform rtPanelInventory;
    private RectTransform rtPanelCrafting;
    private RectTransform rtPanelCharacter;
    Vector4[] open = new Vector4[3];
    Vector4[] close = new Vector4[3];

    public bool picked;
    public UiItemInventory slotPick;
    public UiItemInventory slotDrop;
    public Vector2 mousePos;
    public RectTransform contentInventory;

    private void Awake()
    {
        inventory = player.GetComponent<Inventory>();
        playerController = player.GetComponent<PlayerController>();
        
        panelGral = GetComponent<CanvasGroup>();
        rtPanelInventory = rmPanelInventory.GetComponent<RectTransform>();
        rtPanelCrafting = rtPanelInventory.GetComponent<RectTransform>();
        rtPanelCharacter = rtPanelInventory.GetComponent<RectTransform>();
    }
    void Start()
    {
        Invoke(nameof(LoadInventoryUI), 0.1f);

        playerController.onInventory += OnInventory;
    } 
    void LoadInventoryUI()
    {
        CreateButtonsSlots();
        RefreshAllButtons();
        ResizeContent();

        panelGral.alpha = 0;
        panelGral.interactable = false;
        panelGral.blocksRaycasts = false;
    }
    void ResizeContent()
    {
        int cantChild = contentInventory.transform.childCount;
        GridLayoutGroup grid = contentInventory.GetComponent<GridLayoutGroup>();

        float cellSize = grid.cellSize.y;
        cellSize += grid.spacing.y;
        int columns = grid.constraintCount;

        int currentColumn = 0;
        while (cantChild % columns != 0)
        {
            cantChild++;
            currentColumn++;
            if (currentColumn > columns)
            {
                Debug.LogError("Supera el Maximo de Columnas ", gameObject);
                break; // Salida de de emergencia de While
            }
        }
        int padding = grid.padding.bottom + grid.padding.top;

        contentInventory.sizeDelta = new Vector2(contentInventory.sizeDelta.x, cantChild * cellSize / columns + padding);
    }
    void CreateButtonsSlots()
    {
        int invSize = inventory.GetSize();
        for (int i = 0; i < invSize; i++)
        {
            Slot slot = inventory.GetSlot(i);
            Button newButton = Instantiate(prefabItemInventory, contentInventory);
            newButton.name = ("Slot" + i);
            UiItemInventory item = newButton.GetComponent<UiItemInventory>();
            item.uiInv = this;
            item.SetButton(i, slot.ID);
        }
    }
    void OnInventory()
    {
        RefreshAllButtons();
        switch (inventaryStatus)
        {
            case InventaryStatus.Close:
                playerController.playerInput = false;
                inventaryStatus = InventaryStatus.Opening;
                StartCoroutine(OpeningInventory());

                break;
            case InventaryStatus.Open:
                playerController.playerInput = true;
                inventaryStatus = InventaryStatus.Closeing;
                StartCoroutine(OpeningInventory());

                break;
            case InventaryStatus.Closeing:
            case InventaryStatus.Opening:
            default:
                break;
        }
    }
    // Padding:     || x = Left || z = Right || w = Top || y = Bottom ||
    IEnumerator OpeningInventory()
    {
        float maxDuration = (inventoryOpenDuration > panelOpacityOn) ? inventoryOpenDuration : panelOpacityOn;

        open[0] = rmPanelInventory.padding;
        close[0] = rmPanelInventory.padding;
        close[0].w = rtPanelInventory.rect.height;
        rmPanelInventory.padding = close[0];

        open[1] = rmPanelCrafting.padding;
        close[1] = rmPanelCrafting.padding;
        close[1].z = rtPanelCrafting.rect.width;
        rmPanelCrafting.padding = close[1];

        open[2] = rmPanelCharacter.padding;
        close[2] = rmPanelCharacter.padding;
        close[2].x = rtPanelCharacter.rect.width;
        rmPanelCharacter.padding = close[2];

        switch (inventaryStatus)
        {
            case InventaryStatus.Opening:
                while (onTime < maxDuration)
                {
                    float deltaTime = Time.deltaTime;
                    onTime += deltaTime;
                    onTime2 += deltaTime;
                    float lerpTime;
                    if (onTime < inventoryOpenDuration || onTime > 0)
                    {
                        lerpTime = onTime / inventoryOpenDuration;
                        ModifyPadding(rmPanelInventory, close[0], Vector4.zero, lerpTime);
                        ModifyPadding(rmPanelCrafting, close[1], Vector4.zero, lerpTime);
                        ModifyPadding(rmPanelCharacter, close[2], Vector4.zero, lerpTime);
                    }
                    if (onTime2 < panelOpacityOn || onTime2 > 0)
                    {
                        lerpTime = onTime2 / panelOpacityOn;
                        panelGral.alpha = lerpTime;
                    }
                    yield return null;
                }
                inventaryStatus = InventaryStatus.Open;
                InventaryOn(true);
                break;
            
            case InventaryStatus.Closeing:
                InventaryOn(false);
                onTime2 = panelOpacityOn;

                while (onTime < maxDuration)
                {
                    float deltaTime = Time.deltaTime;
                    if (onTime < inventoryOpenDuration || onTime > 0)
                    {
                        onTime += deltaTime;
                        float lerpTime = onTime / inventoryOpenDuration;
                        ModifyPadding(rmPanelInventory, open[0], close[0], lerpTime);
                        ModifyPadding(rmPanelCrafting, open[1], close[1], lerpTime);
                        ModifyPadding(rmPanelCharacter, open[2], close[2], lerpTime);
                    }

                    if (onTime2 > 0)
                    {
                        onTime2 -= deltaTime;
                        float lerpTime = onTime2 / panelOpacityOn;
                        panelGral.alpha = lerpTime;
                    }
                    yield return null;
                }
                inventaryStatus = InventaryStatus.Close;
                break;
            default:
                break;
        }
        onTime = 0;
        onTime2 = 0;
    }
    void ModifyPadding(RectMask2D mask, Vector4 start, Vector4 end, float time)
    {
        mask.padding = Vector4.Lerp(start, end, time);
    }
    void InventaryOn(bool status)
    {
        panelGral.interactable = status;
        panelGral.blocksRaycasts = status;
        panelGral.alpha = status ? 1 : 0; 
        Cursor.lockState = status ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = status;
    }
    // -----------------------------------------------
    public void RefreshAllButtons()
    {
        onRefreshAllButtonsEvent?.Invoke();
    }
    public string RefreshToolTip(RectTransform btn)
    {
        UiItemInventory uiItem = btn.GetComponent<UiItemInventory>();

        toolTip.transform.position = new Vector3(btn.transform.position.x, btn.transform.position.y, btn.transform.position.z);
        int id = uiItem.GetID();

        string text = TextFormatter(uiItem, id);
        TextMeshProUGUI textMesh = toolTip.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        textMesh.text = text;
        return text;
    }
    public void SwapButtonsIDs()
    {
        inventory.SwapItem(slotPick.GetIndex(), slotDrop.GetIndex());

        slotPick.Refresh();
        slotDrop.Refresh();
    }
    public void MouseEnterOver(RectTransform btn)
    {
        string text = RefreshToolTip(btn);

        int lines = 0;
        int chars = 0;
        int maxChar = 0;
        float offset = 58;
        float margin = 30;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\n')
            {
                lines++;
                if (maxChar < chars)
                    maxChar = chars;
                chars = 0;
            }
            else
            {
                chars++;
            }
        }

        RectTransform aux = toolTip.GetComponent<RectTransform>();
        toolTip.GetComponent<RectTransform>().sizeDelta = new Vector2(aux.sizeDelta.x, lines * offset + margin);
    }

    string TextFormatter(UiItemInventory UiSlot, int idItem)
    {
        int index = UiSlot.GetIndex();
        Slot slot;

        slot = inventory.GetSlot(index);

        if (idItem < 0)
        {
            toolTip.gameObject.SetActive(false);
            return "";
        }
        Item myItem = ItemManager.GetInstance().GetItemFromID(idItem);

        string text = myItem.ItemToString();
        if (myItem.maxStack > 1)
        {
            text += "\nAmount: " + slot.amount;
        }
        return text;
    }
}