using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlantCycle : MonoBehaviour
{
    private ResourceManager resourceManager;

    [SerializeField] private ParticleSystem growthParticles;
    private bool _isGrowing = false;
    public bool isGrowing { get { return _isGrowing; } }

    private float _currentGrowth = 0f;
    public float currentGrowth { get { return _currentGrowth; } }
    private float _growTime = 10f;
    public float growTime { get { return _growTime; } }
    private int sellValue = 5;

    private Renderer meshRenderer;
    private bool playerInRange = false;

    private float tendCooldown = 1f;
    private float lastTendTime = -999f;

    //private PlayerInventory playerInventory;
    //TendingDevice wateringCan;

    //void Update()
    //{
    //    if (playerInRange && Input.GetKeyDown(KeyCode.Q))
    //    {
    //        if (Time.time >= lastTendTime + tendCooldown)
    //        {
    //            TendPlant(0.25f);
    //            lastTendTime = Time.time;
    //        }
    //    }
    //}
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

        _growTime = seed.growTime;
        sellValue = seed.sellPrice;

        _isGrowing = true;
        _currentGrowth = 0f;

        StartCoroutine(GrowPlant());
    }

    //public void SetInventory(PlayerInventory inventory)
    //{
    //    playerInventory = inventory;
    //}

    IEnumerator GrowPlant()
    {
        while (_currentGrowth < _growTime)
        {
            _currentGrowth += Time.deltaTime;

            if (meshRenderer != null && _currentGrowth > _growTime * 0.5f)
                meshRenderer.enabled = true; // visible halfway

            yield return null;
        }

        _isGrowing = false;
        transform.localScale *= 1.5f; // visual indicator of full growth
    }
    //private void OnMouseDown()
    //{
    //    if(!isGrowing) return;

    //    wateringCan = playerInventory.GetToolByName("Watering Can");

    //    if (playerInventory != null && playerInventory.containsTool(wateringCan))
    //    {
    //        if (playerInRange && Time.time >= lastTendTime + tendCooldown)
    //        {
    //            growthParticles.Play();
    //            Debug.Log("Play particle effect on plant interaction.");

    //            TendPlant(0.25f);
    //            lastTendTime = Time.time;
    //        }
    //        else
    //        {
    //            Debug.Log("Wait for tend cooldown!");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("You need a watering can to tend this plant!");
    //    }

    //}   
    private void OnTriggerEnter(Collider other) => playerInRange = true;
    private void OnTriggerExit(Collider other) => playerInRange = false;
    public void TendPlant(float percentReduction)
    {
        if (!_isGrowing) return;

        float remainingTime = _growTime - _currentGrowth;

        float reductionAmount = remainingTime * percentReduction;

        _growTime -= reductionAmount;

        Debug.Log("Plant tended! Reduced remaining time by " + (percentReduction * 100f) + "%");
    }
}