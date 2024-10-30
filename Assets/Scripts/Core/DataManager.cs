using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[DefaultExecutionOrder(-100)] 

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    //public EventSO saveDataEvent;
    //这是后面要加的监听脚本

    private List<ISaveable> saveableList = new List<ISaveable>();

    public GameData saveData;

    private string jsonFolder;

    //private Dictionary<string, GameData> saveDataDict = new Dictionary<string, GameData>();

    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }

        saveData = new GameData();

        jsonFolder = Application.persistentDataPath + "/SAVE/";
    }

    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        var resultPath = jsonFolder + "data.sav";
        Debug.Log(resultPath);
        var jsonSetting = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        var jsonData = JsonConvert.SerializeObject(saveData, Formatting.Indented, jsonSetting);

        if (!File.Exists(resultPath)) 
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData);
    }

    public void Load()
    {
        var resultPath = jsonFolder + "data.sav";

        if (!File.Exists(resultPath))
        {
            return;
        }

        var stringData = File.ReadAllText(resultPath);

        var jsonData = JsonConvert.DeserializeObject<GameData>(stringData);

        saveData = jsonData;

        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }

    private void OnStartNewGame()
    {
        var resultPath = jsonFolder + "data.sav";
        if (File.Exists(resultPath))
        {
            File.Delete(resultPath);
        }
    }
}
