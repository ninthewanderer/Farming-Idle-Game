using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopToggle : MonoBehaviour
{
    public CanvasGroup seedShopUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            seedShopUI.gameObject.SetActive(!seedShopUI.gameObject.activeSelf);
        }
    }
}
