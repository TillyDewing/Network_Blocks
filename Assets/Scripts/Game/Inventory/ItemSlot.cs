using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Item item = ItemDatabase.nullItem;
    public Button button;
    public Image icon;
    public Text count;
    public GameObject higlight;

    public ItemInfo itemInfo;
    public Inventory inventory;
    public int slotId;

    public bool useInfoVeiw = false;

    public void SetItem(Item item)
    {
        this.item = item;

        if (item.ID != -1)
        {
            icon.enabled = true;
            icon.sprite = ItemIcons.GetIcon(item.IconId);
            if (item.Stackable)
            {
                count.text = item.count.ToString();
            }
        }
        else
        {
            icon.sprite = null;
            icon.enabled = false;
            count.text = null;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
        //Debug.Log("Pointer Entered");
        if (item.ID != -1 & useInfoVeiw)
        {
            itemInfo.gameObject.SetActive(true);
            itemInfo.SetItem(item);
            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Pointer Exited");
        if (item.ID != -1 & useInfoVeiw)
        {
            itemInfo.SetItem(ItemDatabase.nullItem);
            itemInfo.gameObject.SetActive(false);
            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == 0)
        {
            if (item.ID != -1)
            {
                if (inventory.selectedSlot != null)
                {
                    inventory.selectedSlot.higlight.SetActive(false);
                }
                Debug.Log("Selected " + item.Title);
                higlight.SetActive(true);
                inventory.selectedSlot = this;
                

            }
        }


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == 0)
        {
            Debug.Log("Mouse Up on: " + item.Title);
            if (inventory.selectedSlot != this && inventory.selectedSlot != null)
            {
                Debug.Log("Swapping");
                inventory.Swap(slotId, inventory.selectedSlot.slotId);
                inventory.selectedSlot.higlight.SetActive(false);
                inventory.selectedSlot = null;
            }
        }


    }
}
