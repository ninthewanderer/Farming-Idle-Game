using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlotSign : MonoBehaviour
{
    private GameObject createdPrompt;
    public GameObject promptPrefab;
    public GameObject plotPrefab;
    public GameObject camera;
    public int gridSize = 1;
    public float spacing = 2f;
    public float plotCost = 500f;
    public int IDnum;

    private MoneyManager moneyManager;
    private SaveManager saveManager;
    private PlotManager plotManager;

    private bool plotBought;
    private bool playerInRange;
    private Coroutine promptCoroutine;
    private TMP_Text priceText;

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
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (promptCoroutine != null)
            {
                StopCoroutine(promptCoroutine);
                Destroy(createdPrompt);
            }
            
            TryBuyPlots();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            if (promptCoroutine == null && !plotBought)
            {
                StartCoroutine(ShowPurchasePrompt());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private IEnumerator ShowPurchasePrompt()
    {
        // Creates the prompt & changes its text as needed.
        createdPrompt = Instantiate(promptPrefab, transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
        priceText = createdPrompt.transform.GetChild(1).GetComponent<TMP_Text>();

        if (plotCost <= 999)
            priceText.text = plotCost.ToString("$000");
        else if (plotCost <= 9999)
            priceText.text = plotCost.ToString("$0,000");
        else if (plotCost <= 99999)
            priceText.text = plotCost.ToString("$00,000");
        else if (plotCost <= 999999)
            priceText.text = plotCost.ToString("$000,000");
        else if (plotCost <= 9999999)
            priceText.text = plotCost.ToString("$0,000,000");
        else
            priceText.text = plotCost.ToString("$00,000,000");
        
        
        // The loop continues while the player is in range and is not interacting with the sign.
        while (playerInRange && !Input.GetKeyDown(KeyCode.E) && !plotBought)
        {
            // Constantly moves the prompt text to face the camera.
            Vector3 cameraPos = camera.transform.position;
            cameraPos.y = createdPrompt.transform.position.y;
            createdPrompt.transform.GetChild(0).transform.LookAt(cameraPos);
            createdPrompt.transform.GetChild(0).transform.Rotate(0, 180, 0);
            createdPrompt.transform.GetChild(1).transform.LookAt(cameraPos);
            createdPrompt.transform.GetChild(1).transform.Rotate(0, 180, 0);

            yield return null;
        }
        
        Destroy(createdPrompt);
        promptCoroutine = null;
    }
    
    private void TryBuyPlots()
    {
        if (moneyManager == null) return;

        try
        {
            SignEvent?.Invoke();

            moneyManager.SpendMoney(plotCost);
            plotManager.RegisterPurchase(IDnum);
            plotBought = true;
            
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