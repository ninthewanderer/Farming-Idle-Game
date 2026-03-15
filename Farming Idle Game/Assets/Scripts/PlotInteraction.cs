using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotInteraction : MonoBehaviour, IInteractable
{
    [Header("Prefabs")]
    [SerializeField] private GameObject plantPrefab;
    [SerializeField] private GameObject dirtMoundPrefab;

    [Header("Grid Settings")]
    [SerializeField] private int gridSize = 3;
    [SerializeField] private float plantSpacing = 1.0f;

    private bool hasPlant = false;
    private bool playerInRange = false;

    private List<GameObject> plants = new List<GameObject>();
    private List<GameObject> dirtMounds = new List<GameObject>();
    private List<PlantCycle> plantCycles = new List<PlantCycle>();

    private MoneyManager moneyManager;

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
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
        int half = gridSize / 2;

        for (int x = -half; x <= half; x++)
        {
            for (int z = -half; z <= half; z++)
            {
                Vector3 offset = new Vector3(x * plantSpacing, 0, z * plantSpacing);

                
                GameObject dirt = Instantiate(
                    dirtMoundPrefab,
                    transform.position + offset + new Vector3(0, 0.1f, 0),
                    Quaternion.identity
                );

                dirt.transform.parent = transform;
                dirtMounds.Add(dirt);

                
                GameObject newPlant = Instantiate(
                    plantPrefab,
                    transform.position + offset + new Vector3(0, 0.4f, 0),
                    Quaternion.identity
                );

                newPlant.transform.parent = transform;
                plants.Add(newPlant);

                PlantCycle cycle = newPlant.GetComponent<PlantCycle>();
                if (cycle != null)
                {
                    plantCycles.Add(cycle);
                }
            }
        }

        hasPlant = true;
    }

    bool AllPlantsGrown()
    {
        foreach (PlantCycle plant in plantCycles)
        {
            if (plant.isGrowing)
                return false;
        }

        return true;
    }

    void Harvest()
    {
        moneyManager.AddMoney(5 * plants.Count);

        foreach (GameObject plant in plants)
        {
            if (plant != null)
                Destroy(plant);
        }

        foreach (GameObject dirt in dirtMounds)
        {
            if (dirt != null)
                Destroy(dirt);
        }

        plants.Clear();
        dirtMounds.Clear();
        plantCycles.Clear();

        hasPlant = false;

        Debug.Log("Plants harvested!");
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
        {
            return "Press [E] to plant!";
        }
        else if (AllPlantsGrown())
        {
            return "Press [E] to harvest!";
        }
        else
        {
            return "Growing...";
        }
    }
}