using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleVolume : MonoBehaviour
{
    private AudioSource musicSource;
    public TextMeshProUGUI volumeText;
    public Slider volumeSlider;
    private int musicVolume;
    
    private delegate void OnVolumeChanged(int value);
    private OnVolumeChanged sensitivityDelegate;
    private event Action<int> VolumeEvent;
    
    void Start()
    {
        // Obtains audio source component and subscribes to the volume change event.
        musicSource = GetComponent<AudioSource>();
        VolumeEvent += OnVolumeChange;
        
        // Ensures volume always starts at the maximum value set in volumeSlider.
        musicVolume = (int)volumeSlider.maxValue;
        VolumeEvent?.Invoke(musicVolume);
    }
    
    // Changes the volume of the music.
    private void OnVolumeChange(int value)
    {
        musicSource.volume = (float)value / 100;
        Debug.Log("Event has been triggered.");
        Debug.Log(value);
    }
    
    // Activated by the "Save" button in the UI -- fires the volume event and changes the music volume.
    public void InvokeVolumeEvent()
    {
        musicVolume = (int)volumeSlider.value;
        VolumeEvent?.Invoke(musicVolume);
    }
    
    // Changes the text on the screen.
    public void ChangeVolumeText()
    {
        volumeText.text = volumeSlider.value.ToString();
    }
}
