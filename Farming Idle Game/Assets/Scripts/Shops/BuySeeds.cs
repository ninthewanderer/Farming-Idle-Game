using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuySeeds : MonoBehaviour
{
    public PlayerInventory inventory;
    public MoneyManager playerMoney;

    // Drag the seed assets here
    public SeedData carrotSeed;
    public SeedData tomatoSeed;
    public SeedData potatoSeed;
    public SeedData watermelonSeed;
    public SeedData blueberrySeed;
    public SeedData strawberrySeed;
    public SeedData turnipSeed;
    public SeedData bellpepperSeed;
    public SeedData spinachSeed;
    public SeedData onionSeed;
    public SeedData cornSeed;
    public SeedData lettuceSeed;

    // Generic buy function using the asset's own price
    public void BuySeed(SeedData seed)
    {
        if (!playerMoney.CanAfford(seed.price))
        {
            Debug.Log("Not enough money to buy " + seed.seedName);
            return;
        }

        playerMoney.SpendMoney(seed.price);
        inventory.AddSeed(seed, 1);

        Debug.Log("Bought " + seed.seedName);
    }

    // Optional wrapper functions for UI buttons
    public void BuyCarrotSeed() => BuySeed(carrotSeed);
    public void BuyTomatoSeed() => BuySeed(tomatoSeed);
    public void BuyPotatoSeed() => BuySeed(potatoSeed);
    public void BuyWatermelonSeed() => BuySeed(watermelonSeed);
    public void BuyBlueberrySeed() => BuySeed(blueberrySeed);
    public void BuyStrawberrySeed() => BuySeed(strawberrySeed);
    public void BuyTurnipSeed() => BuySeed(turnipSeed);
    public void BuyBellpepperSeed() => BuySeed(bellpepperSeed);
    public void BuySpinachSeed() => BuySeed(spinachSeed);
    public void BuyOnionSeed() => BuySeed(onionSeed);
    public void BuyCornSeed() => BuySeed(cornSeed);
    public void BuyLettuceSeed() => BuySeed(lettuceSeed);
}
