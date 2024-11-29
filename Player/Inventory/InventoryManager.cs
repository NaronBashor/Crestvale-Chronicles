using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;

    public GameObject inventoryItemPrefab;

    int selectedSlot = -1;

    public Item[] startingItems;
    public Item[] allItems;

    private int index;

    private void Start()
    {
        index = 0;
        ChangeSelectedSlot(0);
        ContinueGame();
    }

    private void Update()
    {
        // Change selected slot using number keys (1-8)
        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 9) {
                ChangeSelectedSlot(number - 1);
            }
        }

        // Change selected slot using mouse scroll
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0) {
            if (scroll > 0) {
                // Scroll up: Move to the next slot, but don't go beyond the last slot
                if (selectedSlot > 0) {
                    ChangeSelectedSlot(selectedSlot - 1);
                }
            } else if (scroll < 0) {
                // Scroll down: Move to the previous slot, but don't go before the first slot
                if (selectedSlot < 7) {
                    ChangeSelectedSlot(selectedSlot + 1);
                }
            }
        }
    }

    bool IsNewGame()
    {
        return !PlayerPrefs.HasKey("SaveGameExists");
    }

    [ContextMenu("Delete Player Prefs")]
    private void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    // Start a new game
    void StartNewGame()
    {
        // Clear any previous save data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("SaveGameExists", 1); // Mark save as existing

        AssignStartingTools();
    }

    // Continue an existing game
    void ContinueGame()
    {
        if (!IsNewGame()) {
            // Load game data (assuming you have a load function)
            LoadInventory();
        } else {
            Debug.Log("No save file found, starting new game.");
            StartNewGame();
        }
    }

    private void AssignStartingTools()
    {
        foreach (var item in startingItems) {
            bool result = AddItem(startingItems[index]);
            if (result) {
                index++;
            } else {
                //Debug.Log("Item was not added.");
            }
        }
    }

    private void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < item.stackSize) {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    public void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) {
            Item item = itemInSlot.item;
            if (use) {
                itemInSlot.count--;
                if (itemInSlot.count <= 0) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }
            }
            return item;
        } else {
            return null;
        }
    }

    public void SaveInventory()
    {
        InventoryData inventoryData = new InventoryData();

        // Iterate over all inventory slots and store the item data, including the slot index
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null) {
                InventorySlotData slotData = new InventorySlotData {
                    slotIndex = i,                  // Save the slot index
                    itemName = itemInSlot.item.itemName, // Save the item name
                    count = itemInSlot.count         // Save the item count
                };

                inventoryData.slots.Add(slotData);
            }
        }

        // Convert the inventory data to JSON
        string jsonData = JsonUtility.ToJson(inventoryData, true);

        // Save the JSON data to a file
        System.IO.File.WriteAllText(Application.persistentDataPath + "/inventoryData.json", jsonData);
        //Debug.Log("Inventory saved!");
    }

    public void LoadInventory()
    {
        // Check if the save file exists
        string filePath = Application.persistentDataPath + "/inventoryData.json";
        if (System.IO.File.Exists(filePath)) {
            // Load the JSON data from the file
            string jsonData = System.IO.File.ReadAllText(filePath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(jsonData);

            // Clear the current inventory slots
            foreach (var slot in inventorySlots) {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null) {
                    Destroy(itemInSlot.gameObject); // Clear existing items
                }
            }

            // Repopulate the inventory slots with the saved data
            foreach (var slotData in inventoryData.slots) {
                Item item = FindItemByName(slotData.itemName);  // Find the Item object by name
                if (item != null) {
                    InventorySlot slot = inventorySlots[slotData.slotIndex];  // Get the exact saved slot
                    SpawnNewItem(item, slot);  // Spawn the item into the correct slot
                    InventoryItem newItem = slot.GetComponentInChildren<InventoryItem>();
                    newItem.count = slotData.count;  // Restore the item count
                    newItem.RefreshCount();          // Refresh the display
                }
            }

            //Debug.Log("Inventory loaded!");
        } else {
            //Debug.Log("No save file found.");
        }
    }

    public Item FindItemByName(string itemName)
    {
        foreach (Item item in allItems) {
            if (item.itemName == itemName) {
                return item;
            }
        }
        Debug.LogWarning("Item not found: " + itemName);
        return null;
    }

    public Item FindCropToSpawn(string searchString)
    {
        // Known suffixes to strip from search string
        string[] suffixes = { "seed", "seeds" };

        // Remove suffixes from the search string
        foreach (string suffix in suffixes) {
            if (searchString.ToLower().EndsWith(suffix.ToLower())) {
                searchString = searchString.Substring(0, searchString.Length - suffix.Length);
                break; // Stop after removing one suffix
            }
        }

        // First, try to find an exact match (after removing suffix)
        foreach (Item item in allItems) {
            if (item.itemName.Equals(searchString, System.StringComparison.OrdinalIgnoreCase)) {
                return item;
            }
        }

        // If no exact match was found, try to find a partial match
        foreach (Item item in allItems) {
            if (item.itemName.ToLower().Contains(searchString.ToLower())) {
                return item;
            }
        }

        Debug.LogWarning("Item not found for exact or partial match: " + searchString);
        return null;
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<InventorySlotData> slots = new List<InventorySlotData>();
    }

    [System.Serializable]
    public class InventorySlotData
    {
        public int slotIndex;   // The slot's index
        public string itemName;  // Name of the item
        public int count;        // Number of items in the slot
    }

}
