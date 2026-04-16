using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public MoneyManager moneyManager;

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public void Save()
    {
        SaveData data = new SaveData();
        data.money = moneyManager.GetMoney();

        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saved to: " + SavePath);
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        moneyManager.CurrentMoney = data.money;
        Debug.Log("Loaded money: " + data.money);
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