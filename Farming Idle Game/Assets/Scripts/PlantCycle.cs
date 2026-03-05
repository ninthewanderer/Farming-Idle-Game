using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCycle : MonoBehaviour
{
    private bool _isGrowing = false;
    public bool isGrowing { get { return _isGrowing; } }
    // Serialized so we can see if it's working in real time
    [SerializeField] private int growthTime;
    [SerializeField] private int clickReduction;
    [SerializeField] private float currentGrowth = 0f;

    private bool playerInRange = false;
    void OnEnable()
    {
        _isGrowing = true;
        StartCoroutine(PlantGrowth());
    }

    IEnumerator PlantGrowth()
    {
        while (currentGrowth < growthTime)
        {
            currentGrowth += Time.deltaTime;
            yield return null; // wait one frame
        }
        Debug.Log("Plant has fully grown!");
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        _isGrowing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }
    void OnMouseDown()
    {
        if (!_isGrowing || !playerInRange) return;

        currentGrowth += clickReduction; // reduce remaining time
    }
}

