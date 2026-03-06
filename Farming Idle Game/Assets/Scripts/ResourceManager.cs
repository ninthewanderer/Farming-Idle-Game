using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script Manages permanent and temporary crop/seed traits
public class ResourceManager : MonoBehaviour
{
    //add moneyManager from MoneyManager Script
    public MoneyManager moneyManager;

    // Permanent crop traits (Such as Cost of seed and Time to grow)
    [System.Serializable]
    public class CropDefinition
    {
        public string CropName;
        public float SeedCost;
        public float PlantValue;
        public float GrowTime;


        public CropDefinition(string cropName, float seedCost, float plantValue, float growTime)
        {
            CropName = cropName;
            SeedCost = seedCost;
            PlantValue = plantValue;
            GrowTime = growTime;
        }
    }

    // Temporary crop traits (Inventory system)
    [System.Serializable]
    public class CropState
    {
        public int SeedInventory;
        public int PlantInventory;

        public CropState(int seeds = 0, int plants = 0)
        {
            SeedInventory = seeds;
            PlantInventory = plants;
        }
    }

    // Dictionaries to manage the registering of crops
    public Dictionary<string, CropDefinition> CropDefinitions = new Dictionary<string, CropDefinition>();
    public Dictionary<string, CropState> CropStates = new Dictionary<string, CropState>();


    void Start()
    {
        //reference to money manager script for selling/buying seeds and crops
        moneyManager = FindObjectOfType<MoneyManager>();
        // Register crops (I added random crops with random values as an example, feel free to change)
        RegisterCrop("Carrot", 5f, 15f, 30f);
        RegisterCrop("Potato", 10f, 30f, 60f);
        RegisterCrop("Onion", 20f, 60f, 120f);

        //Examples for calling these functions

        BuySeeds("Carrot", 5);
        PlantSeed("Potato");
        HarvestCrop("Onion", 3);
        SellCrop("Carrot", 3);
    }


    // Register crop type (Links temporary and permanent crop values)
    public void RegisterCrop(string name, float seedCost, float plantValue, float growTime)
    {
        CropDefinitions[name] = new CropDefinition(name, seedCost, plantValue, growTime);
        CropStates[name] = new CropState();
    }


    // Add seeds to inventory
    public void AddSeeds(string cropName, int amount)
    {
        if (CropStates.ContainsKey(cropName))
        {
            CropStates[cropName].SeedInventory += amount;
        }
    }


    // Plant seed (Remove from inventory)
    public bool PlantSeed(string cropName)
    {
        if (CropStates.ContainsKey(cropName))
        {
            CropState state = CropStates[cropName];

            if (state.SeedInventory > 0)
            {
                state.SeedInventory--;
                return true;
            }
        }

        return false;
    }


    // Harvest crop (Add to inventory)
    public void HarvestCrop(string cropName, int amount)
    {
        if (CropStates.ContainsKey(cropName))
        {
            CropStates[cropName].PlantInventory += amount;
        }
    }


    // Get sell value
    public float GetPlantValue(string cropName)
    {
        if (CropDefinitions.ContainsKey(cropName))
        {
            return CropDefinitions[cropName].PlantValue;
        }

        return 0f;
    }


    // Get grow time
    public float GetGrowTime(string cropName)
    {
        if (CropDefinitions.ContainsKey(cropName))
        {
            return CropDefinitions[cropName].GrowTime;
        }

        return 0f;
    }

    //Subtract total money and add seeds to inventory
    public bool BuySeeds(string cropName, int amount)
    {
        if (!CropDefinitions.ContainsKey(cropName))
            return false;

        float cost = CropDefinitions[cropName].SeedCost * amount;

        if (moneyManager.SpendMoney(cost))
        {
            AddSeeds(cropName, amount);
            return true;
        }

        return false;
    }

    //Add total money and subtract crops from inventory
    public bool SellCrop(string cropName, int amount)
    {
        if (!CropStates.ContainsKey(cropName))
            return false;

        CropState state = CropStates[cropName];

        if (state.PlantInventory >= amount)
        {
            state.PlantInventory -= amount;

            float value = CropDefinitions[cropName].PlantValue * amount;
            moneyManager.AddMoney(value);

            return true;
        }

        return false;
    }
}