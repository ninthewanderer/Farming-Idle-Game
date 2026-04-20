using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UpgradeManager : MonoBehaviour
{
    // Dictionary with upgrade type name and lists of upgrades
    public Dictionary<string, List<Upgrade>> upgradesDictionary = new Dictionary<string, List<Upgrade>>();

    // Upgrade types
    public ToolUpgrade toolUpgrades;
    public SpeedUpgrade speedUpgrades;

    // Money
    public MoneyManager playerMoney;

    // Inventory
    public PlayerInventory inventory;



    void Start()
    {
        LoadUpgrades();
    }

    void LoadUpgrades()
    {
        // display upgrades in debug for now, place in UI later
        speedUpgrades.LoadUpgrades(this);
        toolUpgrades.LoadUpgrades(this);
    }

    void PurchaseUpgrade(Upgrade upgrade)
    {
        // TODO: purchase logic, add event to update upgrades list after purchase
        //      ^ same with buying tool
    }
}