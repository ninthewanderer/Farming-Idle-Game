using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpeedUpgrade : Upgrade
{
    
    public SpeedUpgrade(string upgradeName, float upgradeCost, string description, float level, UpgradeState state) : base(upgradeName, upgradeCost, description, level, state)
    {
        UpgradeName = upgradeName;
        UpgradeCost = upgradeCost;
        Description = description;
        Level = level;
        State = state;
    }

    // Returns the current availability state of the upgrade
    public UpgradeState CheckUpgrade(UpgradeManager upgradeManager)
    {
        int currentLevel = PlayerPrefs.GetInt("SpeedUpgradeLevel", 0);

        if (currentLevel >= Level)
        {
            return UpgradeState.Purchased;
        }
        else if (upgradeManager.playerMoney.CanAfford(UpgradeCost) && currentLevel + 1 == Level)
        {
            return UpgradeState.Available;
        }
        else
        {
            return UpgradeState.Locked;
        }
    }

    public override void LoadUpgrades(UpgradeManager upgradeManager)
    {
        
        
        List<Upgrade> speedUpgrades = new List<Upgrade>()
        {
            new SpeedUpgrade("Speed Upgrade 1", 500f, "Increases speed by 10%", 1, UpgradeState.Locked),
            new SpeedUpgrade("Speed Upgrade 2", 900f, "Increases speed by 20%", 2, UpgradeState.Locked),
            new SpeedUpgrade("Speed Upgrade 3", 1200f, "Increases speed by 30%", 3, UpgradeState.Locked)
        };

        upgradeManager.upgradesDictionary["Speed"] = speedUpgrades;
        foreach (SpeedUpgrade upgrade in speedUpgrades)
        {
            upgrade.State = upgrade.CheckUpgrade(upgradeManager);
            Debug.Log($"{upgrade.UpgradeName} state: {upgrade.State}");
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }
}
