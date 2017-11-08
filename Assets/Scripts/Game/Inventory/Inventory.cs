using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Item[] items;
    public int size;

    public GameObject inventoryPanel;
    public GameObject slotPanel;
    public GameObject slot;
    public ItemSlot[] slots;

    public ItemInfo itemInfo;

    public ItemSlot selectedSlot;

    void Start()
    {
        items = new Item[size];
        slots = new ItemSlot[size];

        for (int i = 0; i < size; i++)
        {
            items[i] = ItemDatabase.nullItem;
            GameObject tempSlot = Instantiate(slot, slotPanel.transform) as GameObject;   
            tempSlot.transform.localScale = new Vector3(1, 1, 1);
            slots[i] = tempSlot.GetComponent<ItemSlot>();
            slots[i].itemInfo = itemInfo;
            slots[i].inventory = this;
            slots[i].slotId = i;
        }

        AddItem(0, 10);
        AddItem(1, 1);
        RemoveItem(0, 2);
    }

    public bool AddItem(int itemId, int count)
    {
        Item item = ItemDatabase.GetItem(itemId);
        item.count = count;
        
        if (item.ID == -1) //if were trying to add a null item
        {
            Debug.LogError("Error Adding Item: Item does not exist");
            return false;
        }
        Debug.Log(item.Title);

        int emptySlot = -1; //the first empty slot we find in the inventory

        if (item.Stackable) //if its stackable we try to find a stack to add it to if not we add it to the first open slot
        {
            for (int i = 0; i < items.Length; i++)
            {
                //Debug.Log(i + ":" + items[i].ID);
                if (items[i].ID == itemId) //if item is found in iventory update the count
                {
                    items[i].count += count;
                    slots[i].SetItem(items[i]);
                    return true;
                }
                else if(items[i].ID == -1)
                {
                    if (emptySlot == -1)  //if we find an empty spot and we haven't already found one we set empty slot to i
                    {
                        emptySlot = i;
                    }
                }
            }
            if (emptySlot != -1)
            {
                //Debug.Log(emptySlot);
                items[emptySlot] = item; //if we can't find it in the inventory
                slots[emptySlot].SetItem(items[emptySlot]);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].ID == -1)
                {
                    items[i] = item;
                    slots[i].SetItem(items[i]);
                    return true;
                }
            }
        }
        return false; //No space in inventory

    }

    public bool RemoveItem(int itemId, int count)
    {
        Item item = ItemDatabase.GetItem(itemId);
        item.count = count;

        if (item.ID == -1) //if were trying to add a null item
        {
            Debug.LogError("Error Removing Item: Item does not exist");
            return false;
        }

        if (item.Stackable)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].ID == itemId) //if item is found in iventory update the count
                {
                    if (items[i].count > count)
                    {
                        items[i].count -= count;
                        slots[i].SetItem(items[i]);
                        return true;
                    }
                    else if (items[i].count == count)
                    {
                        items[i] = ItemDatabase.nullItem;
                        slots[i].SetItem(ItemDatabase.nullItem);

                        return true;
                    }

                }
            }
             return false;
        }
        else
        {

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].ID == itemId)
                {
                    items[i] = ItemDatabase.nullItem;
                    slots[i].SetItem(ItemDatabase.nullItem);
                    return true;
                }
            }
        }

        return false;
    }

    public void Swap(int slotA, int slotB)
    {
        Item tempItem = items[slotA];
        items[slotA] = items[slotB];
        items[slotB] = tempItem;

        slots[slotA].SetItem(items[slotA]);
        slots[slotB].SetItem(items[slotB]);

    }
}
