using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTools : MonoBehaviour
{
    public PlayerInventory inventory;
    public MoneyManager playerMoney;

    // Drag the tool assets here
    public TendingDevice wateringCan; 


    // Generic function to buy tool
    public void BuyTendingDevice(TendingDevice device)
    {
        if (!playerMoney.CanAfford(device.ToolCost))
        {
            Debug.Log("Not enough money to buy " + device.ToolName);
            return;
        }
        else if(inventory.containsTool(device))
        {
            Debug.Log("You already own " + device.ToolName);
            return;
        }
        playerMoney.SpendMoney(device.ToolCost);
        inventory.addTool(device, 1);
        Debug.Log("Bought " + device.ToolName);
    }

    // Wrapper functions
    public void BuyWateringCan() => BuyTendingDevice(wateringCan);
}
