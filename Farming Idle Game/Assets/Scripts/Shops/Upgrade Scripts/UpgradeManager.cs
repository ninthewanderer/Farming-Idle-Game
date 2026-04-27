using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

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
    public BuyTools buyTools;


    // Shop interaction
    [SerializeField] private GameObject upgradeShop;
    //private bool playerInRange = false;
    //private ShopManager shopManager;
    [Header("Upgrade Buttons")]
    public Button toolUpgradeOne;
    public Button toolUpgradeTwo;
    public Button toolUpgradeThree;
    public Button speedUpgradeOne;
    public Button speedUpgradeTwo;
    public Button speedUpgradeThree;



    void Start()
    {
        LoadUpgrades();

        // subscribe to buy tool event
        GameObject seedShop = GameObject.Find("SeedShopUI");
        buyTools = seedShop.GetComponent<BuyTools>();
        buyTools.purchaseDevice += HandleToolPurchase;

        // adding listeners for button clicks
        toolUpgradeOne.onClick.AddListener(() => PurchaseToolUpgrade((ToolUpgrade)upgradesDictionary["Watering Can"][0]));
        toolUpgradeTwo.onClick.AddListener(() => PurchaseToolUpgrade((ToolUpgrade)upgradesDictionary["Watering Can"][1]));
        toolUpgradeThree.onClick.AddListener(() => PurchaseToolUpgrade((ToolUpgrade)upgradesDictionary["Watering Can"][2]));
        speedUpgradeOne.onClick.AddListener(() => PurchaseSpeedUpgrade((SpeedUpgrade)upgradesDictionary["Speed"][0]));
        speedUpgradeTwo.onClick.AddListener(() => PurchaseSpeedUpgrade((SpeedUpgrade)upgradesDictionary["Speed"][1]));
        speedUpgradeThree.onClick.AddListener(() => PurchaseSpeedUpgrade((SpeedUpgrade)upgradesDictionary["Speed"][2]));
    }

    void LoadUpgrades()
    {
        // display upgrades in debug for now, place in UI later
        speedUpgrades.LoadUpgrades(this);
        toolUpgrades.LoadUpgrades(this);
    }

    public void HandleToolPurchase(TendingDevice device)
    {
        foreach(ToolUpgrade upgrade in upgradesDictionary["Watering Can"])
        {
            upgrade.TendingDevice = device;
            Debug.Log("Updated " + upgrade.UpgradeName + " with new " + device.ToolName);
        }
    }

    public void PurchaseSpeedUpgrade(SpeedUpgrade upgrade)
    {
        // TODO: purchase logic, add event to update upgrades list after purchase
        //      ^ same with buying tool
        if (!playerMoney.CanAfford(upgrade.UpgradeCost))
        {
            Debug.Log("Not enough money to purchase " + upgrade.UpgradeName);
            return;
        }
        else if (upgrade.State == Upgrade.UpgradeState.Purchased)
        {
            Debug.Log("You already purchased " + upgrade.UpgradeName + "!");
            return;
        }
        playerMoney.SpendMoney(upgrade.UpgradeCost);
        Debug.Log("Purchased " + upgrade.UpgradeName);

        int currLevel = PlayerPrefs.GetInt("SpeedUpgradeLevel");
        PlayerPrefs.SetInt("SpeedUpgradeLevel", currLevel + 1);
        upgrade.State = upgrade.CheckUpgrade(this);

    }

    public void PurchaseToolUpgrade(ToolUpgrade upgrade)
    {
        if (!playerMoney.CanAfford(upgrade.UpgradeCost))
        {
            Debug.Log("Not enough money to purchase " + upgrade.UpgradeName);
            return;
        }
        else if (upgrade.TendingDevice == null)
        {
            Debug.Log("You need a Watering Can to purchase " + upgrade.UpgradeName + "!");
            return;
        }
        else if (upgrade.State == Upgrade.UpgradeState.Purchased)
        {
            Debug.Log("You already purchased " + upgrade.UpgradeName + "!");
            return;
        }
        playerMoney.SpendMoney(upgrade.UpgradeCost);
        Debug.Log("Purchased " + upgrade.UpgradeName);

        upgrade.TendingDevice.Level++;
        upgrade.State = upgrade.CheckUpgrade(upgrade.TendingDevice, this);

    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("SpeedUpgradeLevel", 0);
        PlayerPrefs.Save();
    }

}