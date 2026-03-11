using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Entity_Stats playerStats;
    public List<Inventory_EquipmentSlot> equipmentList;

    protected override void Awake()
    {
        base.Awake();
        playerStats = GetComponent<Entity_Stats>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
        var matchingSlots = equipmentList.FindAll(slot => slot.slotType == item.itemData.itemType);

        foreach(var slot in matchingSlots)
        {
            if(slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }

        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equippedItem;

        EquipItem(inventoryItem, slotToReplace);
        UnequipItem(itemToUnequip);
    }
    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        slot.equippedItem = itemToEquip;
        slot.equippedItem.AddModifiers(playerStats);

        RemoveItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip)
    {
        if(CanAddItem() == false)
            return;

        foreach(var slot in equipmentList)
        {
            if(slot.equippedItem == itemToUnequip)
            {
                slot.equippedItem = null;
                break;
            }
        }
        itemToUnequip.RemoveModifiers(playerStats);
        AddItem(itemToUnequip);
    }
}
