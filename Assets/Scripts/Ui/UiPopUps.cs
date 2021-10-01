using System.Collections.Generic;
using UnityEngine;
public class UiPopUps : MonoBehaviour
{
    private Inventory inventory;
    public UiPopUpPickItem itemPopUp;
    [SerializeField] private float timeShow = 1.5f;
    [HideInInspector] public List<UiPopUpPickItem> uiPopList = new List<UiPopUpPickItem>();
    public int maxSizeList = 4;

    private void Awake()
    {
        inventory = GameManager.Get().player.GetComponent<Inventory>();
    }
    private void Start()
    {
        inventory.OnPickUp += PickItem;
    }
    private void PickItem(Item item, int amount)
    {
        UiPopUpPickItem itempick = Instantiate(itemPopUp, transform);
        itempick.SetValues(item.icon, item.itemName, amount.ToString(), this);
        itempick.maxTime = timeShow;
        uiPopList.Add(itempick);

        CheckMaxList();
    }
    void CheckMaxList()
    {
        if (maxSizeList > uiPopList.Count)
            uiPopList.RemoveAt(0);
    }
}