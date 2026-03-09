using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TMP_Text text;
    public double value;

    string[] suffixes = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No" };

    void Update()
    {
        text.text = FormatNumber(value);
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
            return value.ToString("0.##e0");
        }
    }
}
