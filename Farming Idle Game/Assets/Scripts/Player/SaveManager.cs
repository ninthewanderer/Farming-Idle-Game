using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public MoneyManager moneyManager;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public void Save()
    {
        // ---- LAB 12 WORK ----
        SaveData data = new SaveData();
        data.money = moneyManager.GetMoney();

        //string json = JsonUtility.ToJson(data, prettyPrint: true);
        //File.WriteAllText(SavePath, json);
        //Debug.Log("Saved to: " + SavePath);


        // ---- LAB 13 WORK ----
        PlayerPrefs.SetFloat("Money", data.money);
        PlayerPrefs.Save();
        if (PlayerPrefs.HasKey("Money"))
        {
            Debug.Log("Money saved: " + PlayerPrefs.GetFloat("Money"));
        }
        else
        {
            Debug.LogError("Failed to save money.");
        }
    }

    public void Load()
    {
        // ---- LAB 12 WORK ----
        //if (!File.Exists(SavePath))
        //{
        //    Debug.Log("No save file found.");
        //    return;
        //}

        //string json = File.ReadAllText(SavePath);
        //SaveData data = JsonUtility.FromJson<SaveData>(json);
        //moneyManager.CurrentMoney = data.money;
        //Debug.Log("Loaded money: " + data.money);


        // ---- LAB 13 WORK ----
        if (!PlayerPrefs.HasKey("Money"))
        {
            Debug.Log("No save data found.");
            return;
        }
        moneyManager.CurrentMoney = PlayerPrefs.GetFloat("Money");
        Debug.Log("Loaded money: " + moneyManager.CurrentMoney);
    }
    void Start()
    {
        Load();
    }

    void OnApplicationQuit()
    {
        Save();
    }
}