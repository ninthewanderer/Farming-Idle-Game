using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject PlantPrefab;
    [SerializeField] private GameObject dirtMoundPrefab;
    private bool hasPlant = false;
    private PlantCycle plantCycle;
    UiManager uimanager;

    private bool playerInRange = false;
    private GameObject Plant;
    private GameObject Dirt;
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
        Debug.Log("Interacted with the plot!");
        if (!hasPlant)
        {
            Dirt = Instantiate(dirtMoundPrefab, transform.position + new Vector3(0, 0.1f, 0), Quaternion.identity);
            Plant = Instantiate(PlantPrefab, transform.position + new Vector3(0, 0.4f, 0), Quaternion.identity);
            plantCycle = Plant.GetComponent<PlantCycle>();
            hasPlant = true;
        }
        else if (plantCycle != null && plantCycle.isGrowing == false)
        {
            Harvest();
        } 
        else
        {
            Debug.Log("Plant is still growing!");
        }
    }

    private void Harvest()
    {
        hasPlant = false;
        moneyManager.AddMoney(5);

        if (Plant != null)
            Destroy(Plant);

        if (Dirt != null)
            Destroy(Dirt);

        Debug.Log("Plant has been harvested!");
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }

    public string GetInteractPrompt()
    {
        if (!hasPlant)
        {
            return "Press [E] to plant!";
        } else if ((plantCycle != null && !plantCycle.isGrowing))
        {
            return "Press [E] to harvest!";
        } else
        {
            return "Growing...";
        }
    }
}
