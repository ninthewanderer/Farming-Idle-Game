using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    public MoneyManager moneyManager;
    public PlayerController playerController;
    public ToolUpgrade toolUpgrade;


    // Tool upgrades stored per tool key
    public Dictionary<string, List<ToolUpgrade>> ToolUpgrades = new Dictionary<string, List<ToolUpgrade>>();

    // Player movement speed upgrades in a list (OLD)
    // cami TODO: create a speed upgrade class 
    //public List<Upgrade> SpeedUpgrades = new List<Upgrade>();


    void Awake()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        playerController = FindObjectOfType<PlayerController>();

        RegisterUpgrades();
    }


    void RegisterUpgrades()
    {
        // Temp watering can test upgrades, just 3 for now.
        List<ToolUpgrade> wateringCanUpgrades = new List<ToolUpgrade>()
        {
            new("Watering Can Level 1", 25f, "A basic watering can.", 1f, Upgrade.UpgradeState.Locked),
            new("Watering Can Level 2", 50f, "Increases watering can level to 2.", 2f, Upgrade.UpgradeState.Locked),
            new("Watering Can Level 3", 100f, "Increases watering can level to 3.", 3f, Upgrade.UpgradeState.Locked)
        };

        ToolUpgrades["Watering Can"] = wateringCanUpgrades;

        foreach(ToolUpgrade upgrade in wateringCanUpgrades)
        {
            upgrade.State = upgrade.CheckUpgrade(resourceManager.TendingDevices["Watering Can"]);
            Debug.Log($"{upgrade.UpgradeName} state: {upgrade.State}");
        }

        // TODO: Player speed upgrades, increase speed by percentage

    }


    // Buy seeds using ResourceManager 
    public bool BuySeeds(string cropName, int amount)
    {
        if (resourceManager == null) return false;
        return resourceManager.BuySeeds(cropName, amount);
    }


    // Buy a tool upgrade by tool name and upgrade index (1-based)
    public bool BuyToolUpgrade(string toolName, int upgradeIndex)
    {
        if (!ToolUpgrades.ContainsKey(toolName)) return false;
        var upgrades = ToolUpgrades[toolName];
        if (upgradeIndex < 1 || upgradeIndex >= upgrades.Count) return false;

        var upgrade = upgrades[upgradeIndex];
        if (moneyManager == null) return false;
        if (!moneyManager.SpendMoney(upgrade.UpgradeCost)) return false;

        // Increase the tool level in ResourceManager
        if (resourceManager != null && resourceManager.TendingDevices != null && resourceManager.TendingDevices.ContainsKey(toolName))
        {
            var device = resourceManager.TendingDevices[toolName];
            device.Level += 1;
            device.TimeReduction = device.Level * 0.1f; // each level reduces time by 10%
            Debug.Log($"{toolName} upgraded. New level: {device.Level}");
        }
        else
        {
            Debug.Log($"{toolName} upgrade bought but device not found in ResourceManager.");
        }

        return true;
    }


    // Cami TODO: implement player speed upgrades, increase speed by percentage
    // Buy a speed upgrade by index (0-based)

    // -- (OLD CODE) --

    //public bool BuySpeedUpgrade(int upgradeIndex)
    //{
    //    if (playerController == null) return false;
    //    if (upgradeIndex < 0 || upgradeIndex >= SpeedUpgrades.Count) return false;

    //    var upgrade = SpeedUpgrades[upgradeIndex];
    //    if (!moneyManager.SpendMoney(upgrade.UpgradeCost)) return false;

    //    float current = playerController.GetMoveSpeed();
    //    // Increase speed, multiply by 1 + percentage
    //    float newSpeed = current * (1f + upgrade.EffectValue);
    //    playerController.SetMoveSpeed(newSpeed);

    //    Debug.Log($"Speed upgraded from {current} to {newSpeed}");
    //    return true;
    //}
}
