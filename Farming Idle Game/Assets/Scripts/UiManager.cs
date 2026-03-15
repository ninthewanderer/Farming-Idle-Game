using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TMP_Text text;

    private MoneyManager moneyManager;

    string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No" };

    void Start()
    {
        moneyManager = FindObjectOfType<MoneyManager>();
    }

    void Update()
    {
        text.text = FormatNumber(moneyManager.GetMoney());
    }

    string FormatNumber(double num)
    {
        int suffixIndex = 0;

        while (num >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            num /= 1000;
            suffixIndex++;
        }

        if (suffixIndex < suffixes.Length - 1 || num < 1000)
        {
            return num.ToString("0.#") + suffixes[suffixIndex];
        }
        else
        {
            return num.ToString("0.##e0");
        }
    }
}