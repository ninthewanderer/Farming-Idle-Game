using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public enum UpgradeState
    {
        Locked,
        Available,
        Purchased
    }

    public string UpgradeName;
    public float UpgradeCost;
    public string Description;
    // maybe dont need this anymore with the tools
    public float Level; // current lvl/tier of upgrade
    public UpgradeState State;

    public Upgrade(string upgradeName, float upgradeCost, string description, float level, UpgradeState state)
    {
        UpgradeName = upgradeName;
        UpgradeCost = upgradeCost;
        Description = description;
        Level = level;
        State = state;
    }

    // implement methods to change state of upgrade
    //public abstract void UnlockUpgrade();

    //public abstract void PurchaseUpgrade();

    //public abstract void LockUpgrade();

    public abstract void LoadUpgrades(UpgradeManager upgradeManager);
}
