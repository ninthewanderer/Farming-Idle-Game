using System;
using System.Collections.Generic;
using UnityEngine;

public class PlotSign : MonoBehaviour
{
    public float interactRange = 3f;
    public GameObject plotPrefab;
    public int gridSize = 1;
    public float spacing = 2f;
    public float plotCost = 500f;
    public int IDnum;

    private MoneyManager moneyManager;
    private SaveManager saveManager;
    private PlotManager plotManager;

    private event Action SignEvent;

    private static List<PlotSign> allSigns = new List<PlotSign>();

    private void Awake()
    {
        allSigns.Add(this);

        // Normal gameplay: checks money + increases prices
        SignEvent += () => IncreaseOtherSignPrices(true);
    }

    private void OnDestroy()
    {
        allSigns.Remove(this);
    }

    private void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
        saveManager = FindObjectOfType<SaveManager>();
        plotManager = FindObjectOfType<PlotManager>();

        if (moneyManager == null)
            Debug.LogError("No MoneyManager found!");

        if (saveManager == null)
            Debug.LogError("No SaveManager found!");

        if (plotManager == null)
            Debug.LogError("No PlotManager found!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null) return;

            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= interactRange)
            {
                TryBuyPlots();
            }
        }
    }

    private void TryBuyPlots()
    {
        if (moneyManager == null) return;

        try
        {
            SignEvent?.Invoke();

            moneyManager.SpendMoney(plotCost);

            plotManager.RegisterPurchase(IDnum);

            SpawnPlots();

            Destroy(gameObject);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // LOAD VERSION (no money check, but still increases prices)
    public void ForceLoadPurchase()
    {
        try
        {
            IncreaseOtherSignPrices(false);

            SpawnPlots();

            Destroy(gameObject);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void SpawnPlots()
    {
        Vector3 startPos = transform.position -
            new Vector3(spacing * (gridSize - 1) / 2, 0, spacing * (gridSize - 1) / 2);

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 spawnPos = startPos + new Vector3(x * spacing, 0, z * spacing);
                Instantiate(plotPrefab, spawnPos, Quaternion.identity);
            }
        }
    }

    // UPDATED: can skip money check during load
    private void IncreaseOtherSignPrices(bool checkMoney = true)
    {
        if (checkMoney)
        {
            if (moneyManager == null)
            {
                Debug.LogError("MoneyManager is NULL during price check");
                return;
            }

            if (!moneyManager.CanAfford(plotCost))
            {
                throw new Exception("Not enough money to buy plots!");
            }
        }

        foreach (PlotSign sign in allSigns)
        {
            if (sign != this)
            {
                sign.plotCost += 500f;
            }
        }
    }
}