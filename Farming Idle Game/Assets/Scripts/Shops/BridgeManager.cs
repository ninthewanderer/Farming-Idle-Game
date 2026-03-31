using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    public float interactRange = 3f;
    public GameObject bridgeObject;  
    public float BridgeCost = 2000f;

    private MoneyManager moneyManager;
    private GameObject player;

    private static List<BridgeManager> allBridges = new List<BridgeManager>();

    private void Awake()
    {
        allBridges.Add(this);
    }

    private void OnDestroy()
    {
        allBridges.Remove(this);
    }

    private void Start()
    {
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= interactRange)
            {
                TryBuyBridge();
            }
        }
    }

    private void TryBuyBridge()
    {
        if (moneyManager != null)
        {
            if (moneyManager.CanAfford(BridgeCost))
            {
                moneyManager.SpendMoney(BridgeCost);
                EnableBridge();
                Destroy(gameObject);
                IncreaseOtherBridgePrices();
            }
            else
            {
                Debug.Log("Not enough money to buy bridge!");
            }
        }
    }

    private void EnableBridge()
    {
        if (bridgeObject != null)
        {
            bridgeObject.SetActive(true);
        }
    }

    private void IncreaseOtherBridgePrices()
    {
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