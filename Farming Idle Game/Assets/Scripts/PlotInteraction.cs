using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotInteraction : MonoBehaviour, IInteractable
{
    [Header("Prefabs")]
    [SerializeField] private GameObject dirtMoundPrefab;

    [Header("Grid Settings")]
    [SerializeField] private int gridSize = 3;
    [SerializeField] private float plantSpacing = 1.0f;

    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;

    private bool hasPlant = false;
    private bool playerInRange = false;

    private List<GameObject> plants = new List<GameObject>();
    private List<GameObject> dirtMounds = new List<GameObject>();
    private List<SeedData> plantedSeeds = new List<SeedData>();
    private List<PlantCycle> plantCycles = new List<PlantCycle>();

    private MoneyManager moneyManager;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
        if (playerInventory == null)
            Debug.LogError("PlayerInventory reference missing on PlotInteraction!");
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (!hasPlant)
        {
            PlantGrid();
        }
        else if (AllPlantsGrown())
        {
            Harvest();
        }
        else
        {
            Debug.Log("Plants are still growing!");
        }
    }

    void PlantGrid()
    {
        SeedData selectedSeed = playerInventory.GetSelectedSeed();

        if (selectedSeed == null)
        {
            Debug.Log("No seed selected!");
            return;
        }

        int half = gridSize / 2;

        for (int x = -half; x <= half; x++)
        {
            for (int z = -half; z <= half; z++)
            {
                Vector3 offset = new Vector3(x * plantSpacing, 0, z * plantSpacing);

                // Spawn dirt mound
                GameObject dirt = Instantiate(
                    dirtMoundPrefab,
                    transform.position + offset + new Vector3(0, 0.1f, 0),
                    Quaternion.identity
                );
                dirt.transform.parent = transform;
                dirtMounds.Add(dirt);

                // Spawn plant prefab from seed asset
                GameObject newPlant = Instantiate(
                    selectedSeed.plantPrefab,
                    transform.position + offset + new Vector3(0, 0.4f, 0),
                    Quaternion.identity
                );
                newPlant.transform.parent = transform;
                plants.Add(newPlant);

                // Configure PlantCycle on prefab
                PlantCycle cycle = newPlant.GetComponent<PlantCycle>();
                if (cycle != null)
                {
                    cycle.SetSeed(selectedSeed); // assign seed info to the cycle
                    plantCycles.Add(cycle);
                }

                // Track planted seed
                plantedSeeds.Add(selectedSeed);
            }
        }

        // Remove seeds from inventory
        int plantsToPlant = gridSize * gridSize;
        for (int i = 0; i < plantsToPlant; i++)
        {
            playerInventory.UseSelectedSeed();
        }

        hasPlant = true;
    }

    bool AllPlantsGrown()
    {
        foreach (PlantCycle cycle in plantCycles)
        {
            if (cycle.isGrowing)
                return false;
        }
        return true;
    }

    void Harvest()
    {
        int totalMoney = 0;

        foreach (SeedData seed in plantedSeeds)
        {
            totalMoney += seed.sellPrice;
        }

        moneyManager.AddMoney(totalMoney);

        foreach (GameObject plant in plants)
            if (plant != null) Destroy(plant);

        foreach (GameObject dirt in dirtMounds)
            if (dirt != null) Destroy(dirt);

        plants.Clear();
        dirtMounds.Clear();
        plantCycles.Clear();
        plantedSeeds.Clear();

        hasPlant = false;

        Debug.Log("Plants harvested for $" + totalMoney + "!");
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

    public string GetInteractPrompt()
    {
        if (!hasPlant)
            return "Press [E] to plant!";
        else if (AllPlantsGrown())
            return "Press [E] to harvest!";
        else
            return "Growing...";
    }
}