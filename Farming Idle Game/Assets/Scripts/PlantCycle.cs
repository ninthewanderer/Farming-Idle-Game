using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCycle : MonoBehaviour
{
    private ResourceManager resourceManager; // to check for tending device upgrades and apply their effects
    private bool _isGrowing = false;
    public bool isGrowing { get { return _isGrowing; } }
    // Serialized so we can see if it's working in real time
    [SerializeField] private int growthTime = 10;
    [SerializeField] private float clickReduction = 0f;
    [SerializeField] private float currentGrowth = 0f;
    private Renderer MeshRenderer;

    private bool playerInRange = false;
    void OnEnable()
    {
        _isGrowing = true;
        MeshRenderer = GetComponent<Renderer>();
        MeshRenderer.enabled = false; // hide plant until it starts growing, can be replaced with growth stages later
        StartCoroutine(PlantGrowth());
        resourceManager = FindObjectOfType<ResourceManager>();
        if (resourceManager == null)
        {
            Debug.LogError("ResourceManager not found in scene!");
        }
    }

    IEnumerator PlantGrowth()
    {
        while (currentGrowth < growthTime)
        {
            currentGrowth += Time.deltaTime;
            if (currentGrowth > 5f)
            {
                MeshRenderer.enabled = true; // make plant visible after 5 seconds, can be replaced with growth stages later
            }
            yield return null; // wait one frame
        }
        Debug.Log("Plant has fully grown!");
        transform.localScale = transform.localScale * 1.5f; // visually indicate growth, can be replaced with animation later

        _isGrowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
        Debug.Log("In range");
    }
    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }
    void OnMouseDown()
    {
        // has temp check to see if player has any tending device in inventory
        if (!_isGrowing || !playerInRange || resourceManager.TendingDevicesIsEmpty()) return;

        // TEMP watering can. later TODO: apply time reduction based on which device the player is holding and its upgrade level
        clickReduction = 1 + resourceManager.TendingDevices["Watering Can"].TimeReduction; // reduce remaining time by device's reduction percentage
        currentGrowth *= clickReduction; 
        Debug.Log("Reducing time to" + currentGrowth);
    }
}

