using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    public MoneyManager moneyManager;
    public PlayerController playerController;

    [System.Serializable]
    public class Upgrade
    {
        public string UpgradeName;
        public float UpgradeCost;
        public string Description;
        public float EffectValue; // value of upgrade (for exmaple +1 level, +10% speed)

        public Upgrade(string upgradeName, float upgradeCost, string description, float effectValue)
        {
            UpgradeName = upgradeName;
            UpgradeCost = upgradeCost;
            Description = description;
            EffectValue = effectValue; 
        }
    }


    // Tool upgrades stored per tool key
    public Dictionary<string, List<Upgrade>> ToolUpgrades = new Dictionary<string, List<Upgrade>>();

    // Player movement speed upgrades in a list
    public List<Upgrade> SpeedUpgrades = new List<Upgrade>();


    void Start()
    {
        resourceManager = FindObjectOfType<ResourceManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        playerController = FindObjectOfType<PlayerController>();

        RegisterUpgrades();
    }


    void RegisterUpgrades()
    {
        // Watering can upgrades, just two for now. In future will decrease plant growth time by percentage
        List<Upgrade> wateringCanUpgrades = new List<Upgrade>()
        {
            new Upgrade("Watering Can Level 2", 50f, "Increases watering can level to 2.", 1f),
            new Upgrade("Watering Can Level 3", 120f, "Increases watering can level to 3.", 1f)
        };

        ToolUpgrades["Watering Can"] = wateringCanUpgrades;

        // Player speed upgrades, increase speed by percentage
        SpeedUpgrades.Add(new Upgrade("Speed +10%", 30f, "Increase movement speed by 10%.", 0.10f));
        SpeedUpgrades.Add(new Upgrade("Speed +20%", 70f, "Increase movement speed by 20%.", 0.20f));
    }


    // Buy seeds using ResourceManager 
    public bool BuySeeds(string cropName, int amount)
    {
        if (resourceManager == null) return false;
        return resourceManager.BuySeeds(cropName, amount);
    }


    // Buy a tool upgrade by tool name and upgrade index (0-based)
    public bool BuyToolUpgrade(string toolName, int upgradeIndex)
    {
        if (!ToolUpgrades.ContainsKey(toolName)) return false;
        var upgrades = ToolUpgrades[toolName];
        if (upgradeIndex < 0 || upgradeIndex >= upgrades.Count) return false;

        var upgrade = upgrades[upgradeIndex];
        if (moneyManager == null) return false;
        if (!moneyManager.SpendMoney(upgrade.UpgradeCost)) return false;

        // Increase the tool level in ResourceManager
        if (resourceManager != null && resourceManager.TendingDeviceDefinitions != null && resourceManager.TendingDeviceDefinitions.ContainsKey(toolName))
        {
            var device = resourceManager.TendingDeviceDefinitions[toolName];
            device.Level += upgrade.EffectValue;
            Debug.Log($"{toolName} upgraded. New level: {device.Level}");
        }
        else
        {
            Debug.Log($"{toolName} upgrade bought but device not found in ResourceManager.");
        }

        return true;
    }


    // Buy a speed upgrade by index (0-based)
    public bool BuySpeedUpgrade(int upgradeIndex)
    {
        if (playerController == null) return false;
        if (upgradeIndex < 0 || upgradeIndex >= SpeedUpgrades.Count) return false;

        var upgrade = SpeedUpgrades[upgradeIndex];
        if (!moneyManager.SpendMoney(upgrade.UpgradeCost)) return false;

        float current = playerController.GetMoveSpeed();
        // Increase speed, multiply by 1 + percentage
        float newSpeed = current * (1f + upgrade.EffectValue);
        playerController.SetMoveSpeed(newSpeed);

        Debug.Log($"Speed upgraded from {current} to {newSpeed}");
        return true;
    }
}
