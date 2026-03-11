using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Inventory : MonoBehaviour
{
    private Inventory_Player inventory;
    private UI_ItemSlot[] itemSlots;
    private UI_EquipmentSlot[] uiEquipmentSlots;
    [SerializeField] private Transform uiItemSlotParent;
    [SerializeField] private Transform uiEquipmentSlotParent;

    private void Awake()
    {
        itemSlots = uiItemSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        uiEquipmentSlots = uiEquipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

        inventory = FindFirstObjectByType<Inventory_Player>();
        inventory.OnInventoryChange += UpdateUI;

        UpdateUI();
    }

    private void UpdateUI()
    {
        UpdateInventorySlots();
        UpdateEquipmentSlots();
    }

    private void UpdateEquipmentSlots()
    {
        List<Inventory_EquipmentSlot> playerEquipmentList = inventory.equipmentList;

        for (int i = 0; i < uiEquipmentSlots.Length; i++)
        {
            var playerEquipmentSlot = playerEquipmentList[i];

            if(playerEquipmentSlot.HasItem() == false)
                uiEquipmentSlots[i].UpdateSlot(null);
            else
                uiEquipmentSlots[i].UpdateSlot(playerEquipmentSlot.equippedItem);
        }
    }

    private void UpdateInventorySlots()
    {
        List<Inventory_Item> itemList = inventory.itemList;

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if(i < itemList.Count)
            {
                itemSlots[i].UpdateSlot(itemList[i]);
            }
            else
            {
                itemSlots[i].UpdateSlot(null);
            }
        }
    }
}
