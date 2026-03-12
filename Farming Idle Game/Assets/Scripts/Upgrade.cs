using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public string UpgradeName;
    public float UpgradeCost;
    public string Description;
    public float EffectValue; // value of upgrade (for exmaple +1 level, +10% speed)

    public Upgrade(string upgradeName, float upgradeCost, string description, float effectValue)
    {
        UpgradeName = upgradeName;
        UpgradeCost = upgradeCost;
        Description = description;
        EffectValue = effectValue;
    }

    public enum UpgradeState
    {
        Locked,
        Available,
        Purchased
    }


}
