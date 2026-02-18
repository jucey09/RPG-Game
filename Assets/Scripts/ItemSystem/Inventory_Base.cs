using System.Collections.Generic;
using UnityEngine;

public class Inventory_Base : MonoBehaviour
{
    public int maxInventorySize = 10;
    public List<Inventory_Item> itemList = new List<Inventory_Item>();

    public bool CanAddItem(Inventory_Item item) => itemList.Count < maxInventorySize;

    public void AddItem(Inventory_Item itemToAdd)
    {
        itemList.Add(itemToAdd);
    }
}
