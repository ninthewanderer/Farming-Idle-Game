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

    public SeedData GetSelectedSeed()
    {
        if (inventory.Count == 0 || selectedIndex < 0 || selectedIndex >= inventory.Count)
            return null;

        return inventory[selectedIndex].seed;
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
            // Remove the slot
            inventory.RemoveAt(selectedIndex);

            // If inventory is now empty
            if (inventory.Count == 0)
            {
                selectedIndex = 0;
                scrollFlag = true; // allow scrolling again later
                return;
            }

            // If we removed the last slot, wrap to start
            if (selectedIndex >= inventory.Count)
                selectedIndex = 0;
        }
    }



}
