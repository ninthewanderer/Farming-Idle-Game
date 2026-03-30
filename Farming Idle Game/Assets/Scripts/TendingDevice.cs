using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tending Device", menuName = "Tending Device")]
public class TendingDevice : ScriptableObject
{
    public string ToolName;
    public float Level; // current upgrade level/tier. 1-indexed, starts at 1
    public float ToolCost; // cami note to self - I don't think this is needed with cost in ToolUpgrade, leaving for now but consider removing later
    public string Description;
    public float TimeReduction; // reduce grow timer by percentage when player uses tool on a plant
    public TendingDevice(string toolName, float level, float toolCost, string description, float timeReduction)
    {
        ToolName = toolName;
        Level = level;
        ToolCost = toolCost;
        Description = description;
        TimeReduction = timeReduction;
    }
}
