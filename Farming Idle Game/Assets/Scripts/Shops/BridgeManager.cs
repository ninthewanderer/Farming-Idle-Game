using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BridgeManager : MonoBehaviour
{
    private bool playerInRange;
    public GameObject bridgeObject;
    public GameObject promptPrefab;
    private GameObject createdPrompt;
    public float BridgeCost = 2000f;
    public GameObject bridgeBlock;

    private MoneyManager moneyManager;
    private GameObject player;
    private Camera playerCamera;
    private Coroutine promptCoroutine;
    private TMP_Text priceText;
    private bool bridgeBought;
    
    // Lab 13 delegate & event system setup
    private delegate void IncreaseBridges();
    private IncreaseBridges bridgeDelegate;
    private event Action BridgeEvent;

    private static List<BridgeManager> allBridges = new List<BridgeManager>();

    private void Awake()
    {
        allBridges.Add(this);
        
        // Subscribes to the bridge event.
        BridgeEvent += IncreaseOtherBridgePrices;
    }

    private void OnDestroy()
    {
        allBridges.Remove(this);
    }

    private void Start()
    {
        playerCamera = FindObjectOfType<Camera>();
        moneyManager = GameObject.FindObjectOfType<MoneyManager>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (moneyManager == null)
        {
            Debug.LogError("No MoneyManager found in the scene!");
        }

        if (bridgeObject != null)
        {
            bridgeObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Bridge object not assigned in the Inspector!");
        }

        if (bridgeBlock != null)
        {
            bridgeBlock.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            if (promptCoroutine != null)
            {
                Destroy(createdPrompt);
                promptCoroutine = null;
            }

            TryBuyBridge();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;

            if (promptCoroutine == null && !bridgeBought)
            {
                promptCoroutine = StartCoroutine(ShowPurchasePrompt());
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
        createdPrompt = Instantiate(promptPrefab, transform.position + new Vector3(0, 4f, 0), Quaternion.identity);
        priceText = createdPrompt.transform.GetChild(1).GetComponent<TMP_Text>();

        if (BridgeCost <= 999)
            priceText.text = BridgeCost.ToString("$000");
        else if (BridgeCost <= 9999)
            priceText.text = BridgeCost.ToString("$0,000");
        else if (BridgeCost <= 99999)
            priceText.text = BridgeCost.ToString("$00,000");
        else if (BridgeCost <= 999999)
            priceText.text = BridgeCost.ToString("$000,000");
        else if (BridgeCost <= 9999999)
            priceText.text = BridgeCost.ToString("$0,000,000");
        else
            priceText.text = BridgeCost.ToString("$00,000,000");
        
        
        // The loop continues while the player is in range and is not interacting with the sign.
        while (playerInRange && !Input.GetKeyDown(KeyCode.E) && !bridgeBought)
        {
            // Constantly moves the prompt text to face the camera.
            Vector3 playerCameraPos = playerCamera.transform.position;
            playerCameraPos.y = createdPrompt.transform.position.y;
            createdPrompt.transform.GetChild(0).transform.LookAt(playerCameraPos);
            createdPrompt.transform.GetChild(0).transform.Rotate(0, 180, 0);
            createdPrompt.transform.GetChild(1).transform.LookAt(playerCameraPos);
            createdPrompt.transform.GetChild(1).transform.Rotate(0, 180, 0);

            yield return null;
        }
        
        Destroy(createdPrompt);
        promptCoroutine = null;
    }
    
    private void TryBuyBridge()
    {
        if (moneyManager != null)
        {
            // if (moneyManager.CanAfford(BridgeCost))
            // {
            try
            {
                BridgeEvent?.Invoke();
                moneyManager.SpendMoney(BridgeCost);
                bridgeBought = true;
                EnableBridge();
                Destroy(gameObject);
                // IncreaseOtherBridgePrices();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            // }
            // else
            // {
            //     Debug.Log("Not enough money to buy bridge!");
            // }
        }
    }

    private void EnableBridge()
    {
        if (bridgeObject != null)
        {
            bridgeObject.SetActive(true);
        }

        if (bridgeBlock != null)
        {
            bridgeBlock.SetActive(false);
        }
    }

    private void IncreaseOtherBridgePrices()
    {
        // The check for affording is now done here so that the error can be handled using a try-catch in TryBuyBridge().
        if (!moneyManager.CanAfford(BridgeCost))
        {
            throw new Exception("Not enough money to buy bridge!");
        }
        
        foreach (BridgeManager sign in allBridges)
        {
            if (sign != this)
            {
                sign.BridgeCost += 2000f;
                Debug.Log($"Bridge at {sign.transform.position} new price: {sign.BridgeCost}");
            }
        }
    }
}