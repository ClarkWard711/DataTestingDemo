using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class DataInputs : MonoBehaviour
{

    #region 一堆public
    public static DataInputs Instance;

    public enum PlayerJobState { Knight, Samari, Sorcerer, Thief, Watcher }
    [SerializeField] Data.PartyData partyData;

    int playerStartLevel = 1;
    int playerStartSoulLevel = 1;

    public int maxHPOfPlayer;
    public int maxSPOfPlayer;
    public int physicalAttackOfPlayer;
    public int soulAttackOfPlayer;
    public int physicalDefenceOfPlayer;
    public int soulDefenceOfPlayer;
    public int hitOfPlayer;
    public int nimblenessOfPlayer;
    public int speedOfPlayer;
    public int criticalOfPlayer;
    public int meleeOfPlayer;
    public int remoteOfPlayer;
    public int playerCurrentLevel;
    public int playerCurrentSoulLevel;
    public int playerCurrentHP;
    public int playerCurrentSP;
    public int expCurrentLevel;
    public int sxpCurrentLevel;
    public int currentExp;
    public int deltaExp;
    public int deltaSp;
    int index;

    Transform playerTransform;
    public Transform positionTransform;

    public Text level,hp, sp, pa, sa, pd, sd, hit, nim, spd, cri, melee, remote;
    public PlayerJobState state = PlayerJobState.Knight;

    public static bool isClone;

    
    #endregion

    /*[System.Serializable] class SaveData
    {
        public int CurrentLevel = 1;

        public Vector2 playerPosition;
    }*/

    void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().enabled = true;

        playerCurrentLevel = playerStartLevel;
        playerCurrentSoulLevel = playerStartSoulLevel;
        playerTransform = GetComponent<Transform>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        Load();
        UpdatePlayerJob(state);
        //Debug.Log(playerCurrentLevel);
        Save();
        Load();
        if (playerCurrentHP == 0)
        {
            playerCurrentHP = maxHPOfPlayer;
            playerCurrentSP = maxSPOfPlayer;
        }
        
        //playerTransform.position = positionTransform.position;

        

        //Debug.Log(playerCurrentHP);
        
    }

    /*private void Start()
    {
        if (!isClone)
        {
            DontDestroyOnLoad(gameObject);
        }
    }*/

    #region UpdateStats
    public void UpdatePlayerStats(int currentLevel)
    {
        playerCurrentHP = partyData.JobDataList[index].JobStatsList[currentLevel - 1].maxHP;
        playerCurrentSP = partyData.JobDataList[index].JobStatsList[currentLevel - 1].maxSP;

        maxHPOfPlayer           = partyData.JobDataList[index].JobStatsList[currentLevel - 1].maxHP;
        maxSPOfPlayer           = partyData.JobDataList[index].JobStatsList[currentLevel - 1].maxSP;

        physicalAttackOfPlayer  = partyData.JobDataList[index].JobStatsList[currentLevel - 1].physicalAttack;
        soulAttackOfPlayer      = partyData.JobDataList[index].JobStatsList[currentLevel - 1].soulAttack;
        physicalDefenceOfPlayer = partyData.JobDataList[index].JobStatsList[currentLevel - 1].physicalDefence;
        soulDefenceOfPlayer     = partyData.JobDataList[index].JobStatsList[currentLevel - 1].soulDefence;

        hitOfPlayer             = partyData.JobDataList[index].JobStatsList[currentLevel - 1].hit;
        nimblenessOfPlayer      = partyData.JobDataList[index].JobStatsList[currentLevel - 1].nimbleness;
        speedOfPlayer           = partyData.JobDataList[index].JobStatsList[currentLevel - 1].speed;
        criticalOfPlayer        = partyData.JobDataList[index].JobStatsList[currentLevel - 1].critical;

        meleeOfPlayer           = partyData.JobDataList[index].JobStatsList[currentLevel - 1].melee;
        remoteOfPlayer          = partyData.JobDataList[index].JobStatsList[currentLevel - 1].remote;

        expCurrentLevel         = partyData.JobDataList[index].JobStatsList[currentLevel - 1].expPerLevel;
        sxpCurrentLevel         = partyData.JobDataList[index].JobStatsList[currentLevel-1].sxpPerLevel;

        //Debug.Log(maxHPOfPlayer);
        
        Panel.Instance.UpdateDataPanel();
        level.text = playerCurrentLevel.ToString();
        
        hp.text = maxHPOfPlayer.ToString();
        sp.text = maxSPOfPlayer.ToString();
        pa.text = physicalAttackOfPlayer.ToString();
        sa.text = soulAttackOfPlayer.ToString();
        pd.text = physicalDefenceOfPlayer.ToString();
        sd.text = soulDefenceOfPlayer.ToString();
        hit.text = hitOfPlayer.ToString();
        nim.text = nimblenessOfPlayer.ToString();
        spd.text = speedOfPlayer.ToString();
        cri.text = criticalOfPlayer.ToString();
        melee.text = meleeOfPlayer.ToString();
        remote.text = remoteOfPlayer.ToString();
    }

    public void UpdatePlayerJob(PlayerJobState currentState)
    {
        switch (currentState)
        {
            case PlayerJobState.Knight:
                index = 0;
                UpdatePlayerStats(playerCurrentLevel);
                break;
            case PlayerJobState.Samari:
                index = 1;
                UpdatePlayerStats(playerCurrentLevel);
                break;
            case PlayerJobState.Sorcerer:
                index = 2;
                UpdatePlayerStats(playerCurrentLevel);
                break;
            case PlayerJobState.Thief:
                index = 3;
                UpdatePlayerStats(playerCurrentLevel);
                break;
            case PlayerJobState.Watcher:
                index = 4;
                UpdatePlayerStats(playerCurrentLevel);
                break;

        }
    }

    #endregion

    #region SaveAndLoad
    public void Save()
    {
        SaveAndLoad.Instance.playerCurrentLevel = playerCurrentLevel;
        SaveAndLoad.Instance.playerCurrentSoulLevel = playerCurrentSoulLevel;
        //SaveAndLoad.Instance.positionTransform = playerTransform;

        SaveAndLoad.Instance.playerCurrentHP = playerCurrentHP;
        SaveAndLoad.Instance.playerCurrentSP = playerCurrentSP;
        SaveAndLoad.Instance.maxHP = maxHPOfPlayer;
        SaveAndLoad.Instance.maxSP = maxSPOfPlayer;
        SaveAndLoad.Instance.pa = physicalAttackOfPlayer;
        SaveAndLoad.Instance.sa = soulAttackOfPlayer;
        SaveAndLoad.Instance.pd = physicalDefenceOfPlayer;
        SaveAndLoad.Instance.sd = soulDefenceOfPlayer;
        SaveAndLoad.Instance.hit = hitOfPlayer;
        SaveAndLoad.Instance.nim = nimblenessOfPlayer;
        SaveAndLoad.Instance.spd = speedOfPlayer;
        SaveAndLoad.Instance.cri = criticalOfPlayer;
        SaveAndLoad.Instance.melee = meleeOfPlayer;
        SaveAndLoad.Instance.remote = remoteOfPlayer;
        SaveAndLoad.Instance.SaveByJson();
    }

    public void Load()
    {
        SaveAndLoad.Instance.LoadFromJson();
        //positionTransform = SaveAndLoad.Instance.positionTransform;
        //playerCurrentLevel = SaveAndLoad.Instance.playerCurrentLevel;
        playerCurrentHP = SaveAndLoad.Instance.playerCurrentHP;
        playerCurrentSP = SaveAndLoad.Instance.playerCurrentSP;
        deltaExp = SaveAndLoad.Instance.DeltaExp;
        deltaSp = SaveAndLoad.Instance.DeltaSp;
        maxHPOfPlayer = SaveAndLoad.Instance.maxHP;
        maxSPOfPlayer = SaveAndLoad.Instance.maxSP;
    }
    #endregion
}
