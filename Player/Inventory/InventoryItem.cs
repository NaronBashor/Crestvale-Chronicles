using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Image image;
    public TextMeshProUGUI countText;
    private GameObject inventoryPanel; // Reference to the inventory panel

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    private bool canDrag = false;

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inventoryPanel = GameObject.Find("InventoryPanel");
        // Check if the inventory panel is active
        if (inventoryPanel != null && inventoryPanel.activeSelf) {
            canDrag = true; // Allow dragging if inventory is open
            image.raycastTarget = false;
            countText.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        } else {
            canDrag = false; // Disable dragging if inventory is closed
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canDrag) {
            // Only move the item if dragging is allowed
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag) {
            image.raycastTarget = true;
            countText.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
            canDrag = false; // Reset the flag after drag ends
        }
    }
}
