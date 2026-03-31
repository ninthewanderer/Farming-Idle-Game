using System.Collections.Generic;
using UnityEngine;

public class PlotSign : MonoBehaviour
{
    public float interactRange = 3f; //Can change if we decide to use clicking instead of E
    public GameObject plotPrefab;
    public int gridSize = 1;
    public float spacing = 2f;
    public float plotCost = 500f; //set to increase in cost by 500 for every sign bought

    private MoneyManager moneyManager;

    // Keep track of all signs in the scene (to increase their prices when one is bought)
    private static List<PlotSign> allSigns = new List<PlotSign>();

    private void Awake()
    {
        allSigns.Add(this); // Register this sign
    }

    private void OnDestroy()
    {
        allSigns.Remove(this); // Remove when destroyed
    }

    private void Start()
    {
        moneyManager = GameObject.FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogError("No MoneyManager found in the scene!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= interactRange)
                {
                    TryBuyPlots();
                }
            }
        }
    }

    private void TryBuyPlots()
    {
        if (moneyManager != null)
        {
            if (moneyManager.CanAfford(plotCost))
            {
                moneyManager.SpendMoney(plotCost);
                SpawnPlots();
                Destroy(gameObject);
                IncreaseOtherSignPrices();
            }
            else
            {
                Debug.Log("Not enough money to buy plots!");
            }
        }
    }

    private void SpawnPlots()
    {
        Vector3 startPos = transform.position - new Vector3(spacing * (gridSize - 1) / 2, 0, spacing * (gridSize - 1) / 2);

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 spawnPos = startPos + new Vector3(x * spacing, 0, z * spacing);
                Instantiate(plotPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    private void IncreaseOtherSignPrices()
    {
        foreach (PlotSign sign in allSigns)
        {
            
            if (sign != this)
            {
                sign.plotCost += 500f;
                Debug.Log($"Sign at {sign.transform.position} new price: {sign.plotCost}");
            }
        }
    }
}