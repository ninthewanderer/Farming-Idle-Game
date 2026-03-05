using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    [SerializeField] GameObject plotPrefab;
    [SerializeField] Transform[] plotPositions;

    // Start is called before the first frame update
    void Start()
    {
        GameObject firstPlot = Instantiate(plotPrefab, plotPositions[0].position, Quaternion.identity);
    }

    // Will continue to update this to spawn more plots as the player progresses, but for now just one plot to test the planting and harvesting mechanics

}
