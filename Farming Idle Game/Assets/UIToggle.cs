using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    private GameObject seedShopUI;
    void Start()
    {
        seedShopUI = GameObject.Find("SeedShopUI");
        seedShopUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            seedShopUI.SetActive(!seedShopUI.activeSelf);
        }
    }
}
