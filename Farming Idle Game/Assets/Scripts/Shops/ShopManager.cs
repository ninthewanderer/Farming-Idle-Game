using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour, IInteractable
{
    public ResourceManager resourceManager;
    public MoneyManager moneyManager;
    public PlayerController playerController;

    private bool playerInRange = false;
    private bool shopActive;

    public CanvasGroup shopUI;

    // -- (OLD CODE) --

    // Buy seeds using ResourceManager 
    //public bool BuySeeds(string cropName, int amount)
    //{
    //    if (resourceManager == null) return false;
    //    return resourceManager.BuySeeds(cropName, amount);
    //}


    //// Buy a tool upgrade by tool name and upgrade index (1-based)
    //public bool BuyToolUpgrade(string toolName, int upgradeIndex)
    //{
    //    if (!ToolUpgrades.ContainsKey(toolName)) return false;
    //    var upgrades = ToolUpgrades[toolName];
    //    if (upgradeIndex < 1 || upgradeIndex >= upgrades.Count) return false;

    //    var upgrade = upgrades[upgradeIndex];
    //    if (moneyManager == null) return false;
    //    if (!moneyManager.SpendMoney(upgrade.UpgradeCost)) return false;

    //    // Increase the tool level in ResourceManager
    //    if (resourceManager != null && resourceManager.TendingDevices != null && resourceManager.TendingDevices.ContainsKey(toolName))
    //    {
    //        var device = resourceManager.TendingDevices[toolName];
    //        device.Level += 1;
    //        device.TimeReduction = device.Level * 0.1f; // each level reduces time by 10%
    //        Debug.Log($"{toolName} upgraded. New level: {device.Level}");
    //    }
    //    else
    //    {
    //        Debug.Log($"{toolName} upgrade bought but device not found in ResourceManager.");
    //    }

    //    return true;
    //}


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


    // -- NEW CODE 4/22 --

    // Interact with shops

    void Start()
    {
        shopUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Open shop UI
            Interact(shopUI);
            Debug.Log("Shop opened!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    public void Interact()
    {
        // method required by IInteractable, using overloaded CanvasGroup version instead
    }

    public void Interact(CanvasGroup shopUI)
    {
        if (shopUI != null)
        {
            shopUI.gameObject.SetActive(!shopUI.gameObject.activeSelf);
        }
    }

    public string GetInteractPrompt()
    {
        return null;
    }
}
