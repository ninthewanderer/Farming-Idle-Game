using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject PlantPrefab;
    private bool hasPlant = false;
    private PlantCycle plantCycle;
    UiManager uimanager;

    private bool playerInRange = false;
    private GameObject Plant;

    void awake()
    {
       



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
            Plant = Instantiate(PlantPrefab, transform.position + Vector3.up, Quaternion.identity);
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
        uimanager = FindObjectOfType<UiManager>();
        uimanager.value += 5;
        
        Destroy(Plant);
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
            return "Press [E} to harvest!";
        } else
        {
            return "Growing...";
        }
    }
}
