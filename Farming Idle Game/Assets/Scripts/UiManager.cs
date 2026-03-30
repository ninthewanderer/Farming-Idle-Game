using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public PlayerInventory inventory;

    // Seeds
    public Image seedIcon;
    public TMP_Text seedName;
    public TMP_Text seedAmount;

    public TMP_Text text;

    private MoneyManager moneyManager;

    string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No" };

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    void Update()
    {
        text.text = FormatNumber(moneyManager.GetMoney());
        UpdateSeedUI();
    }

    string FormatNumber(double num)
    {
        int suffixIndex = 0;

        while (num >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            num /= 1000;
            suffixIndex++;
        }

        if (suffixIndex < suffixes.Length - 1 || num < 1000)
        {
            return num.ToString("0.#") + suffixes[suffixIndex];
        }
        else
        {
            return num.ToString("0.##e0");
        }
    }

    void UpdateSeedUI()
    {
        SeedData seed = inventory.GetSelectedSeed();
        TendingDevice device = inventory.GetSelectedTool();

        if (seed==null && device==null)
        {
            seedName.text = "";
            seedAmount.text = "";
            seedIcon.enabled = false;
            return;
        }

        InventorySlot slot = inventory.inventory[inventory.selectedIndex];
        // If the slot has a tool
        if (seed==null && device!=null)
        {
            seedName.text = device.ToolName;
            seedIcon.sprite = device.icon;
            seedAmount.text = "Level " + device.Level;
            seedIcon.enabled = true;
        }
        // Else if the slot has a seed
        else
        {
            seedName.text = slot.seed.seedName;
            seedIcon.sprite = seed.icon;
            seedAmount.text = "X" + slot.quantity;
            seedIcon.enabled = true;
        }

    }
}