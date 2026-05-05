using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public float CurrentMoney = 500f;

    public bool SpendMoney(float amount)
    {
        if (CurrentMoney >= amount)
        {
            CurrentMoney -= amount;
            return true;
        }

        return false;
    }

    public void AddMoney(float amount)
    {
        CurrentMoney += amount;
    }

    public float GetMoney()
    {
        return CurrentMoney;
    }

    public bool CanAfford(float amount)
    {
        return CurrentMoney >= amount;
    }
}