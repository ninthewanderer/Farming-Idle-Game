using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendingDevice : MonoBehaviour
{
    public string ToolName;
    public float Level; // current upgrade level
    public float ToolCost;
    public string Description;
    public float TimeReduction; // reduce grow timer by percentage when player uses tool on a plant
    public ToolUpgrade upgrade;
    public TendingDevice(string toolName, float level, float toolCost, string description, float timeReduction)
    {
        ToolName = toolName;
        Level = level;
        ToolCost = toolCost;
        Description = description;
        TimeReduction = timeReduction;
    }
}
