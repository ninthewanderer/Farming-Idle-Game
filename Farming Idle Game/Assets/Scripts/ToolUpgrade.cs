using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolUpgrade : Upgrade
{
    // Using MoneyManager for all money related operations
    private MoneyManager moneyManager;

    // Reference to the device in question
    private TendingDevice tendingDevice;

    public ToolUpgrade(string upgradeName, float upgradeCost, string description, float level, UpgradeState state) : base(upgradeName, upgradeCost, description, level, state)
    {
        UpgradeName = upgradeName;
        UpgradeCost = upgradeCost;
        Description = description;
        Level = level;
        State = state;
    }

    // function to call when player opens shop ui to see if upgrade is available to purchase
    // idea is to be called elsewhere to see what's greyed out (or otherwise marked unavailable) in ui and what's not
    public UpgradeState CheckUpgrade(TendingDevice device)
    {
        bool canAfford = moneyManager.CanAfford(UpgradeCost);

        if (canAfford && device.Level == Level - 1 && State != UpgradeState.Purchased) 
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

    public override void UnlockUpgrade()
    {
        State = UpgradeState.Available;
    }

    public override void LockUpgrade()
    {
        State = UpgradeState.Locked;
    }

    public override void PurchaseUpgrade()
    {
        if (State == UpgradeState.Available)
        {
            moneyManager.SpendMoney(UpgradeCost);
            State = UpgradeState.Purchased;
            // Additional logic to apply the upgrade effect can be added here
        }
    }

    private void Awake()
    {
        State = CheckUpgrade(tendingDevice);
    }
}
