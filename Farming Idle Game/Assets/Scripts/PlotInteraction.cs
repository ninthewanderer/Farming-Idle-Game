using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotInteraction : MonoBehaviour
{
    [SerializeField] private GameObject InteractUI;
    [SerializeField] private GameObject PlantPrefab;
    private bool hasPlant = false;
    private PlantCycle plantCycle;

    private bool playerInRange = false;
    private GameObject Plant;

    void OnEnable()
    {
        InteractUI.SetActive(false);
    }

    void OnDisable()
    {

    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private void Interact()
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

    void Harvest()
    {
        hasPlant = false;
        // Make money go up
        Destroy(Plant);
        Debug.Log("Plant has been harvested!");

    }
    private void OnTriggerEnter(Collider other)
    {
        InteractUI.SetActive(true);
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        InteractUI.SetActive(false);
        playerInRange = false;
    }
}
