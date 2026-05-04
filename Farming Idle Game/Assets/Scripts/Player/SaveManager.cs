using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public MoneyManager moneyManager;
    public PlotManager plotManager;

    [System.Serializable]
    public class SaveData
    {
        public float money;
        public List<int> purchasedPlotIDs = new List<int>();
    }

    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    void Awake()
    {
        if (plotManager == null)
            plotManager = FindObjectOfType<PlotManager>();
    }

    public void Save()
    {
        SaveData data = new SaveData();

        data.money = moneyManager.GetMoney();

        // FIX: use method instead of missing field
        data.purchasedPlotIDs = plotManager.GetPurchasedPlotIDs();

        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey("SaveData"))
        {
            Debug.Log("No save data found.");
            return;
        }

        string json = PlayerPrefs.GetString("SaveData");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        moneyManager.CurrentMoney = data.money;

        // FIX: apply to HashSet system properly
        plotManager.LoadPurchasedPlots(data.purchasedPlotIDs);
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