using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public static SaveAndLoad Instance;

    public int playerCurrentLevel = 1;
    public int playerCurrentSoulLevel = 1;
    public int playerCurrentHP;
    public int playerCurrentSP;
    //public int playerCurrentExp;
    public int DeltaExp;
    public int DeltaSp;
    public int maxHP;
    public int maxSP;

    public int pa;
    public int sa;
    public int pd;
    public int sd;
    public int hit;
    public int nim;
    public int spd;
    public int cri;
    public int melee;
    public int remote;


    //public Transform positionTransform;

    void Awake()
    {
        //positionTransform.position = new Vector2((float)-0.47, (float)0.46);
        Instance = this;
    }

    [System.Serializable]
    public class SaveData
    {
        public int CurrentLevel = 1;
        public int CurrentSoulLevel = 1;
        public int PlayerCurrentHP;
        public int PlayerCurrentSP;
        //public int PlayerCurrentExp;
        public int deltaExp;
        public int deltaSp;
        //public Vector2 playerPosition;
        public int MaxHP;
        public int MaxSP;

        public int Pa;
        public int Sa;
        public int Pd;
        public int Sd;
        public int Hit;
        public int Nim;
        public int Spd;
        public int Cri;
        public int Melee;
        public int Remote;

    }

    public void LoadData(SaveData saveData)
    {
        playerCurrentLevel = saveData.CurrentLevel;
        playerCurrentSoulLevel = saveData.CurrentSoulLevel;
        //positionTransform.position = saveData.playerPosition;
        playerCurrentHP = saveData.PlayerCurrentHP;
        playerCurrentSP = saveData.PlayerCurrentSP;
        maxHP = saveData.MaxHP;
        maxSP = saveData.MaxSP;
        //playerCurrentExp = saveData.PlayerCurrentExp;
        DeltaExp = saveData.deltaExp;
        DeltaSp = saveData.deltaSp;
    }

    public SaveData SavingData()
    {
        var saveData = new SaveData();
        saveData.CurrentLevel = playerCurrentLevel;
        saveData.CurrentSoulLevel = playerCurrentSoulLevel;
        //saveData.playerPosition = positionTransform.position;
        saveData.PlayerCurrentHP = playerCurrentHP;
        saveData.PlayerCurrentSP = playerCurrentSP;
        saveData.MaxHP = maxHP;
        saveData.MaxSP = maxSP;
        //saveData.PlayerCurrentExp = playerCurrentExp;
        saveData.deltaExp = DeltaExp;
        saveData.deltaSp = DeltaSp;
        

        saveData.Pa = pa;
        saveData.Sa = sa;
        saveData.Pd = pd;
        saveData.Sd = sd;
        saveData.Hit = hit;
        saveData.Nim = nim;
        saveData.Spd = spd;
        saveData.Cri = cri;
        saveData.Melee = melee;
        saveData.Remote = remote;
        return saveData;
    }

    public void SaveByJson()
    {
        SaveSystem.SaveByJson("PlayerData.sav", SavingData());
    }

    public void LoadFromJson()
    {
        var SaveData = SaveSystem.LoadFromJson<SaveData>("PlayerData.sav");
        LoadData(SaveData);
    }
}
