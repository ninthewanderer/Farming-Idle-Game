using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


public class PlaytimeCounter : MonoBehaviour
{
    private string _dataPath;
    private string _textFile;
    private float _playTime;
    void Awake()
    {

        _dataPath = Application.persistentDataPath + "/Player_Data/";
        Debug.Log(_dataPath);
        if (!Directory.Exists(_dataPath))
        {
            Directory.CreateDirectory(_dataPath);
        }
        _textFile = _dataPath + "PlayTime_Data.txt";
        if (File.Exists(_textFile))
        {
            Debug.Log(_textFile + " found! Starting playtime!");
            _playTime = 0;
            return;
        }
        else
        {
            Debug.Log("File not found. Creating it!");
            StreamWriter newStream = File.CreateText(_textFile);
            newStream.WriteLine("LOG STARTS\n");
            newStream.Close();
            Debug.Log("New file created!");
            _playTime = 0;
        }
    }

    private void Update()
    {
        _playTime += Time.deltaTime;
    }
    void OnApplicationQuit()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(_playTime);
        string formattedTime = string.Format("{0} hours {1} minutes {2} seconds", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        StreamWriter streamWriter = File.AppendText(_textFile);
        streamWriter.WriteLine("GAME TIME LOG: " + formattedTime);
        streamWriter.Close();
    }

}
