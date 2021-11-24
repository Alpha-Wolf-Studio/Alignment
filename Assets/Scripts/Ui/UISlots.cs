using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISlots : MonoBehaviour
{
    [SerializeField] ArmController armController;
    [Space(10)]
    [SerializeField] Image[] slots;
    [SerializeField] TextMeshProUGUI[] slotNumbers;
    [SerializeField] Color slotColor;
    // Start is called before the first frame update
    private void Awake()
    {
        armController.OnArmChange += ChangeSelectedSlot;
        slots[0].color = slotColor;
        slotNumbers[0].color = slotColor;
    }

    void ChangeSelectedSlot(int selectedSlotIndex) 
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].color = Color.white;
            slotNumbers[i].color = Color.white;
        }
        slots[selectedSlotIndex - 1].color = slotColor;
        slotNumbers[selectedSlotIndex - 1].color = slotColor;
    }
}
