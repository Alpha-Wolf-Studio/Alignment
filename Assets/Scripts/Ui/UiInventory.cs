using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    public enum InventaryStatus { Open, Close, Opening, Closeing }
    public InventaryStatus inventaryStatus = InventaryStatus.Close;
    
    [HideInInspector] public Inventory inventory;
    private PlayerController player;

    // Agarrar un evento para agarrar cuando el Inventario esté totalmente cargado.
    public Action onRefreshAllButtonsEvent;
    public Image slotAux;
    public Button prefabItemInventory;
    public GameObject item;
    public GameObject toolTip;
    public CanvasGroup hud;

    private float onTime;
    private float onTime2;
    public float panelOpacityOn = 1.0f;
    public float inventoryOpenDuration = 1.0f;
    public RectMask2D rmPanelInventory;
    public RectMask2D rmPanelCrafting;
    public RectMask2D rmPanelCharacter;
    public CanvasGroup panelGral;
    private RectTransform rtPanelInventory;
    private RectTransform rtPanelCrafting;
    private RectTransform rtPanelCharacter;
    Vector4[] open = new Vector4[3];
    Vector4[] close = new Vector4[3];

    public bool picked;
    [HideInInspector] public UiItemInventory slotPick;
    [HideInInspector] public UiItemInventory slotDrop;
    public Vector2 mousePos;
    public RectTransform contentInventory;
    private List<UiItemInventory> listUItemInventory = new List<UiItemInventory>();

    private bool loaded;
    private void Awake()
    {
        player = GameManager.Get().player;
        inventory = player.GetComponent<Inventory>();
        
        rtPanelInventory = rmPanelInventory.GetComponent<RectTransform>();
        rtPanelCrafting = rtPanelInventory.GetComponent<RectTransform>();
        rtPanelCharacter = rtPanelInventory.GetComponent<RectTransform>();
    }
    void Start()
    {
        Invoke(nameof(LoadInventoryUI), 0.1f);

        player.onInventory += OnInventory;
    }
    private void Update()
    {
        if (inventaryStatus == InventaryStatus.Open)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnInventory();
            }
        }
    }
    void LoadInventoryUI()
    {
        loaded = true;
        CreateButtonsSlots(0);
        RefreshAllButtons();
        ResizeContent();

        panelGral.alpha = 0;
        panelGral.interactable = false;
        panelGral.blocksRaycasts = false;
    }
    void ResizeContent()
    {
        if (loaded)
        {
            int cantChild = contentInventory.transform.childCount;
            GridLayoutGroup grid = contentInventory.GetComponent<GridLayoutGroup>();

            float cellSize = grid.cellSize.y;
            cellSize += grid.spacing.y;
            int columns = grid.constraintCount;
            int rows = 1;

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

            while (rows < cantChild / columns)
            {
                rows++;
            }

            int padding = grid.padding.bottom + grid.padding.top;

            Vector2 size = contentInventory.sizeDelta;
            size.y = rows * cellSize + 70;
            //Debug.Log("SizeY: " + size.y);
            contentInventory.sizeDelta = size;
        }
    }
    void CreateButtonsSlots(int index)
    {
        int invSize = inventory.GetSize();
        for (int i = index; i < invSize; i++)
        {
            Slot slot = inventory.GetSlot(i);
            Button newButton = Instantiate(prefabItemInventory, contentInventory);
            newButton.name = ("Slot" + i);
            UiItemInventory item = newButton.GetComponent<UiItemInventory>();
            item.uiInv = this;
            item.SetButton(i, slot.ID);
            listUItemInventory.Add(item);
        }
    }
    bool CheckForNewItemsSlots()
    {
        int invSize = inventory.GetSize();
        int uiList = listUItemInventory.Count;

        if (invSize != uiList)
        {
            CreateButtonsSlots(uiList);
            return true;
        }

        return false;
    }
    void OnInventory()
    {
        //float width = DataPersistant.Get().gameSettings.general.GetCurrentResolutionVector2().x;
        //panelGral.GetComponent<RectTransform>().sizeDelta = new Vector2(-width, 0);
        //Debug.Log("sizeDelta: " + panelGral.GetComponent<RectTransform>().sizeDelta);
        //Debug.Log("AnchoredPosition: " + panelGral.GetComponent<RectTransform>().anchoredPosition);
        //Debug.Log("pivot: " + panelGral.GetComponent<RectTransform>().pivot);

        if (CheckForNewItemsSlots())
            Debug.Log("Nuevos Items Agregados.");
        RefreshAllButtons();
        switch (inventaryStatus)
        {
            case InventaryStatus.Close:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.UiOpenInventory))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiOpenInventory), gameObject);

                player.ChangeControllerToInventory();
                inventaryStatus = InventaryStatus.Opening;
                StartCoroutine(OpeningInventory());

                break;
            case InventaryStatus.Open:
                if (Sfx.Get().GetEnable(Sfx.ListSfx.UiCloseInventory))
                    AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiCloseInventory), gameObject);

                player.ChangeControllerToGame();
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
        ResizeContent();
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
                        if (1 - lerpTime > 0.2f)
                            hud.alpha = 1 - lerpTime;
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
                        hud.alpha = 1 - lerpTime;
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
        player.AvailableCursor(status);
    }
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
        if (Sfx.Get().GetEnable(Sfx.ListSfx.UiSwapItem))
            AkSoundEngine.PostEvent(Sfx.Get().GetList(Sfx.ListSfx.UiSwapItem), gameObject);
    }
    public void MouseEnterOver(RectTransform btn)
    {
        string text = RefreshToolTip(btn);

        int lines = 0;
        int chars = 0;
        int maxChar = 0;
        float offset = 58;
        float margin = 60;

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
    string TextFormatter(UiItemInventory uiSlot, int idItem)
    {
        int index = uiSlot.GetIndex();
        Slot slot;

        slot = inventory.GetSlot(index);

        if (idItem < 0)
        {
            toolTip.gameObject.SetActive(false);
            return "";
        }
        Item myItem = ItemManager.Get().GetItemFromID(idItem);

        string text = myItem.name;
        if (myItem.maxStack > 1)
        {
            //text += "\nAmount: " + slot.amount;
        }
        return text;
    }
    public bool DropItem(int index) => inventory.ThrowItem(index);
}