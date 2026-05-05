using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleSensitivity : MonoBehaviour
{
    public GameObject cinemachineCamera;
    public CanvasGroup seedShopCanvasGroup;
    public CanvasGroup sensitivityCanvasGroup;
    public TextMeshProUGUI sensitivityText;
    public Slider sensitivitySlider;
    private int cameraSensitivity;
    
    private CinemachineVirtualCamera virtualCamera;
    private delegate void OnSensitivityChanged(int value);
    private OnSensitivityChanged sensitivityDelegate;
    private event Action<int> SensitivityEvent;
    
    void Start()
    {
        // Obtains virtual camera component and subscribes to the sensitivity change event.
        virtualCamera = cinemachineCamera.GetComponent<CinemachineVirtualCamera>();
        SensitivityEvent += OnSensitivityChange;

        // Ensures camera sensitivity always starts at the minimum value set in sensitivitySlider.
        cameraSensitivity = (int)sensitivitySlider.minValue;
        SensitivityEvent?.Invoke(cameraSensitivity);
    }
    
    void Update()
    {
        // If the player doesn't have the seed shop open when they press escape, this menu will appear.
        if (!sensitivityCanvasGroup.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !seedShopCanvasGroup.isActiveAndEnabled)
            {
                // Stops time so the player and camera cannot move and then enables the sensitivity menu.
                Time.timeScale = 0;
                sensitivityCanvasGroup.gameObject.SetActive(true);
            }
        }
        else
        {
            // Hides the UI if the player presses escape again. Alternative to pressing "Return".
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HideUI();
            }
        }
    }

    // Changes the horizontal and vertical sensitivity on the cinemachine camera.
    private void OnSensitivityChange(int value)
    {
        var pov = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.m_MaxSpeed = value;
        pov.m_VerticalAxis.m_MaxSpeed = value;
    }

    // Changes the text on the screen.
    public void ChangeSensitivityText()
    {
        sensitivityText.text = sensitivitySlider.value.ToString();
    }

    // Activated by the "Save" button in the UI -- fires the sensitivity event and changes the camera sensitivity.
    public void InvokeSensitivityEvent()
    {
        cameraSensitivity = (int)sensitivitySlider.value;
        SensitivityEvent?.Invoke(cameraSensitivity);
    }

    // Activated by the "Return" button in the UI -- simply closes the menu and sets time back to normal.
    public void HideUI()
    {
        sensitivityCanvasGroup.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
