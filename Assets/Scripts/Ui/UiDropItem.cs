using UnityEngine;
using UnityEngine.EventSystems;

public class UiDropItem : MonoBehaviour, IPointerEnterHandler
{
    public UiInventory uiInv;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiInv.picked)
        {
            uiInv.picked = false;
            if (uiInv.DropItem(uiInv.slotPick.GetIndex()))
            {
                uiInv.slotPick.Refresh();
            }
        }
    }
}