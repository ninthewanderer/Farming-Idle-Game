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
    void Awake()
    {

        _dataPath = Application.persistentDataPath + "/Player_Data/";
        Debug.Log(_dataPath);
        _textFile = _dataPath + "Save_Data.txt";
        if (File.Exists(_textFile))
        {
            StreamWriter streamWriter = File.AppendText(_textFile);
            streamWriter.WriteLine("Game ended: " + DateTime.Now);
            streamWriter.Close();
        }
        else
        {
            Debug.Log("File not found--creating it!");
            StreamWriter newStream = File.CreateText(_textFile);
            newStream.WriteLine("LOG STARTS\n");
            newStream.Close();
            Debug.Log("New file created!");
        }
    }

}
