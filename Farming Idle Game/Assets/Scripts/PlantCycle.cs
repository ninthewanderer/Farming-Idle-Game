using System.Collections;
using UnityEngine;

public class PlantCycle : MonoBehaviour
{
    private ResourceManager resourceManager;

    private bool _isGrowing = false;
    public bool isGrowing { get { return _isGrowing; } }

    private float currentGrowth = 0f;
    private float growTime = 10f;
    private int sellValue = 5;

    private Renderer meshRenderer;
    private bool playerInRange = false;

    void OnEnable()
    {
        meshRenderer = GetComponent<Renderer>();
        if (meshRenderer != null)
            meshRenderer.enabled = false; // hide until visible
    }

    public void SetSeed(SeedData seed)
    {
        if (seed == null)
        {
            Debug.LogError("SetSeed called with null seed!");
            return;
        }

        growTime = seed.growTime;
        sellValue = seed.sellPrice;

        _isGrowing = true;
        currentGrowth = 0f;

        StartCoroutine(GrowPlant());
    }

    IEnumerator GrowPlant()
    {
        while (currentGrowth < growTime)
        {
            currentGrowth += Time.deltaTime;

            if (meshRenderer != null && currentGrowth > growTime * 0.5f)
                meshRenderer.enabled = true; // visible halfway

            yield return null;
        }

        _isGrowing = false;
        transform.localScale *= 1.5f; // visual indicator of full growth
    }

    private void OnTriggerEnter(Collider other) => playerInRange = true;
    private void OnTriggerExit(Collider other) => playerInRange = false;
}