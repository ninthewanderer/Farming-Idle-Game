using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

[System.Serializable]
public class ToolUpgrade : Upgrade
{
    // Using MoneyManager for all money related operations
    private MoneyManager moneyManager;

    // Reference to the device in question
    public TendingDevice TendingDevice;

    public ToolUpgrade(string upgradeName, float upgradeCost, string description, float level, UpgradeState state, TendingDevice device) : base(upgradeName, upgradeCost, description, level, state)
    {
        UpgradeName = upgradeName;
        UpgradeCost = upgradeCost;
        Description = description;
        Level = level;
        State = state;
        TendingDevice = device;
    }

    // Returns the current availability state of the upgrade
    public UpgradeState CheckUpgrade(TendingDevice device, UpgradeManager upgradeManager)
    {
        bool canAfford = upgradeManager.playerMoney.CanAfford(UpgradeCost);
        if (device == null)
        {
            return UpgradeState.Locked; 
        }


        if (canAfford && device.Level + 1 == Level && State != UpgradeState.Purchased) 
        {
            return UpgradeState.Available;
        }
        else if (device.Level >= Level)
        {
            return UpgradeState.Purchased;
        }
        else
        { 
            return UpgradeState.Locked;
        }

    }

    public override void LoadUpgrades(UpgradeManager upgradeManager)
    {
        // Temp watering can test upgrades, just 3 for now.
        // note to self (cami): when game starts, player has no tool, so add event so upgrades reload when player buys tool
        TendingDevice = upgradeManager.inventory.GetToolByName("Watering Can");
        List<Upgrade> wateringCanUpgrades = new List<Upgrade>()
        {
            new ToolUpgrade("Watering Can Upgrade 1", 700f, "Increases watering can level to 2.", 1f, Upgrade.UpgradeState.Locked, TendingDevice),
            new ToolUpgrade("Watering Can Upgrade 2", 900f, "Increases watering can level to 3.", 2f, Upgrade.UpgradeState.Locked, TendingDevice),
            new ToolUpgrade("Watering Can Upgrade 3", 1200f, "Increases watering can level to 4.", 3f, Upgrade.UpgradeState.Locked, TendingDevice)
        };



        upgradeManager.upgradesDictionary["Watering Can"] = wateringCanUpgrades;

        foreach (ToolUpgrade upgrade in wateringCanUpgrades)
        {
            upgrade.State = upgrade.CheckUpgrade(TendingDevice, upgradeManager);
            Debug.Log($"{upgrade.UpgradeName} state: {upgrade.State}");
        }
    }


    // TODO: upon purchase increase plant growth speed on click
}
