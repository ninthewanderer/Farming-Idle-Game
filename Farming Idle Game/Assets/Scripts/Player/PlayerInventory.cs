using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<InventorySlot> inventory = new List<InventorySlot>();
    public int selectedIndex = 0;
    public bool scrollFlag;
    public void AddSeed(SeedData seed, int amount)
    {
        foreach (InventorySlot slot in inventory)
        {
            if (slot.seed == seed)
            {
                slot.quantity += amount;
                return;
            }
        }

        inventory.Add(new InventorySlot(seed, amount));

        // If this is the first seed in the inventory, select it
        if (inventory.Count == 1)
        {
            selectedIndex = 0;
        }
    }

    // Add tool to inventory
    public void addTool(TendingDevice tool, int level)
    {
        inventory.Add(new InventorySlot(tool, level));
    }

    public SeedData GetSelectedSeed()
    {
        if (inventory.Count == 0 || selectedIndex < 0 || selectedIndex >= inventory.Count)
            return null;

        return inventory[selectedIndex].seed;
    }

    public TendingDevice GetSelectedTool()
    {
        if (inventory.Count == 0 || selectedIndex < 0 || selectedIndex >= inventory.Count)
            return null;
        return inventory[selectedIndex].device;
    }

    public bool containsTool(TendingDevice device)
    {
        foreach (InventorySlot slot in inventory)
        {
            if (slot.device == device)
            {
                return true;
            }
        }
        return false;
    }

    public void selectNext()
    {
        scrollFlag = false;
        if (inventory.Count == 0) return;

        selectedIndex++;
        if (selectedIndex >= inventory.Count)
            selectedIndex = 0;
    }

    public void SelectPrevious()
    {
        scrollFlag = false;

        if (inventory.Count == 0)
            return;

        selectedIndex--;

        if (selectedIndex < 0)
            selectedIndex = inventory.Count - 1;
    }
    private void Update()
    {
        HandleScrollInput();
    }

    void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f && scrollFlag ==true)
        {
            selectNext();
        }
        if (scroll < 0f && scrollFlag == true)
        {
            SelectPrevious();
        }
        if (scroll == 0f)
        {
            scrollFlag = true;
        }
    }

    public void UseSelectedSeed()
    {
        if (inventory.Count == 0)
            return;

        InventorySlot slot = inventory[selectedIndex];
        slot.quantity--;

        if (slot.quantity <= 0)
        {
            int removedIndex = selectedIndex;

            inventory.RemoveAt(removedIndex);

            if (inventory.Count == 0)
            {
                selectedIndex = 0;
                scrollFlag = true;
                return;
            }

            // Move to the next seed that exists
            selectedIndex = removedIndex;

            if (selectedIndex >= inventory.Count)
                selectedIndex = 0;
        }
    }



}
