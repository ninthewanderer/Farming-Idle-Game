using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public GameObject spendMoneyAnimation;
    public GameObject gainMoneyAnimation;
    public float CurrentMoney = 500f;

    public bool SpendMoney(float amount)
    {
        if (CurrentMoney >= amount)
        {
            spendMoneyAnimation.gameObject.GetComponent<TextMeshProUGUI>().text = amount.ToString("-######00");
            spendMoneyAnimation.SetActive(true);
            CurrentMoney -= amount;
            return true;
        }

        return false;
    }

    public void AddMoney(float amount)
    {
        gainMoneyAnimation.gameObject.GetComponent<TextMeshProUGUI>().text = amount.ToString("+######00");
        gainMoneyAnimation.SetActive(true);
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