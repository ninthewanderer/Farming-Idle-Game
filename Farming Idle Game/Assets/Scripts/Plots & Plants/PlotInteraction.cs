using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlotInteraction : MonoBehaviour, IInteractable
{
    [Header("Prefabs")]
    [SerializeField] private GameObject dirtMoundPrefab;
    [SerializeField] private GameObject timerPrefab;

    //commented out from 3x3 to 1x1 change
   // [Header("Grid Settings")]
   // [SerializeField] private int gridSize = 3;
   // [SerializeField] private float plantSpacing = 1.0f;

    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;

    private bool hasPlant = false;
    private bool playerInRange = false;

    private List<GameObject> plants = new List<GameObject>();
    private List<GameObject> dirtMounds = new List<GameObject>();
    private List<SeedData> plantedSeeds = new List<SeedData>();
    private List<PlantCycle> plantCycles = new List<PlantCycle>();

    private MoneyManager moneyManager;
    
    private Coroutine timerCoroutine;
    private TMP_Text timerText;

    private float tendCooldown = 1f;
    private float lastTendTime = -999f;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();

        if (playerInventory == null)
        {
            GameObject gm = GameObject.Find("GameManager");

            if (gm != null)
            {
                playerInventory = gm.GetComponent<PlayerInventory>();
            }
            else
            {
                Debug.LogError("GameManager not found in scene!");
            }
        }

        if (playerInventory == null)
            Debug.LogError("PlayerInventory component missing on GameManager!");
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void OnMouseDown()
    {
        TendingDevice wateringCan = playerInventory.GetToolByName("Watering Can");
        
        if (hasPlant && !AllPlantsGrown())
        {
            if (playerInventory != null && wateringCan != null)
            {
                float percentReduction = wateringCan.TimeReduction;
                if (Time.time >= lastTendTime + tendCooldown)
                {
                    foreach (PlantCycle cycle in plantCycles)
                    {
                        if (cycle.isGrowing)
                        {
                            cycle.TendPlant(percentReduction);
                        }
                    }
                    lastTendTime = Time.time;
                }
                else
                {
                    Debug.Log("Tending device is on cooldown!");
                }
            }
            else
            {
                Debug.Log("Player does not have a watering can!");
            }
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
            // If the player is still waiting on a plant to grow, its growth time will be shown for 5 seconds.
            if (timerCoroutine == null)
            {
                timerCoroutine = StartCoroutine(GetTimeLeft(5f));
            }
        }
    }
    
    /*original plant grid
     * void PlantGrid() { SeedData selectedSeed = playerInventory.GetSelectedSeed(); if (selectedSeed == null) { Debug.Log("No seed selected!"); return; } int half = gridSize / 2; for (int x = -half; x <= half; x++) { for (int z = -half; z <= half; z++) { Vector3 offset = new Vector3(x * plantSpacing, 0, z * plantSpacing); // Spawn dirt mound GameObject dirt = Instantiate( dirtMoundPrefab, transform.position + offset + new Vector3(0, 0.1f, 0), Quaternion.identity ); dirt.transform.parent = transform; dirtMounds.Add(dirt); // Spawn plant prefab from seed asset GameObject newPlant = Instantiate( selectedSeed.plantPrefab, transform.position + offset + new Vector3(0, 0.4f, 0), Quaternion.identity ); newPlant.transform.parent = transform; plants.Add(newPlant); // Configure PlantCycle on prefab PlantCycle cycle = newPlant.GetComponent<PlantCycle>(); if (cycle != null) { cycle.SetSeed(selectedSeed); // assign seed info to the cycle plantCycles.Add(cycle); } // Track planted seed plantedSeeds.Add(selectedSeed); } } // Remove seeds from inventory int plantsToPlant = gridSize * gridSize; for (int i = 0; i < plantsToPlant; i++) { playerInventory.UseSelectedSeed(); } hasPlant = true; }
      */
    void PlantGrid()
    {
        SeedData selectedSeed = playerInventory.GetSelectedSeed();

        if (selectedSeed == null)
        {
            Debug.Log("No seed selected!");
            return;
        }

        // Spawn dirt mound
        GameObject dirt = Instantiate(
            dirtMoundPrefab,
            transform.position + new Vector3(0, 0.5f, 0),
            Quaternion.identity
        );
        dirt.transform.parent = transform;
        dirtMounds.Add(dirt);

        // Spawn plant
        GameObject newPlant = Instantiate(
            selectedSeed.plantPrefab,
            transform.position + new Vector3(0, 0.8f, 0),
            Quaternion.identity
        );
        newPlant.transform.parent = transform;
        plants.Add(newPlant);

        // Configure plant cycle
        PlantCycle cycle = newPlant.GetComponent<PlantCycle>();
        if (cycle != null)
        {
            cycle.SetSeed(selectedSeed);
            //cycle.SetInventory(playerInventory);
            plantCycles.Add(cycle);
        }

        // Track planted seed
        plantedSeeds.Add(selectedSeed);

        // Remove ONE seed from inventory
        playerInventory.UseSelectedSeed();

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

    // Handles the functionality of the countdown timer for the plant.
    private IEnumerator GetTimeLeft(float popupTime)
    {
        // Elapsed time tracks the time that has passed so that the pop-up disappears once it has reached popupTime.
        float elapsedTime = 0f;
        
        // Stores the children in the prefab (timer text)
        GameObject timer = Instantiate(timerPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
        timerText = timer.transform.GetChild(1).GetComponent<TMP_Text>();
        
        // The loop continues while the plant hasn't finished growing AND the pop-up can still be visible.
        while (plantCycles[0].isGrowing && elapsedTime <= popupTime)
        {
            elapsedTime += Time.deltaTime;
            
            // Constantly moves the timer text to face the player.
            if (camera == null)
            {
                camera = GameObject.FindGameObjectWithTag("Camera");
            }
            Vector3 playerPos = camera.transform.position;
            playerPos.y = timer.transform.position.y;
            timer.transform.GetChild(0).transform.LookAt(playerPos);
            timer.transform.GetChild(0).transform.Rotate(0, 180, 0);
            timer.transform.GetChild(1).transform.LookAt(playerPos);
            timer.transform.GetChild(1).transform.Rotate(0, 180, 0);
            
            float timeRemaining = plantCycles[0].growTime - plantCycles[0].currentGrowth;
            timerText.text = timeRemaining.ToString("0:00");
            
            yield return null;
        }
        
        Destroy(timer);
        timerCoroutine = null;
    }
}