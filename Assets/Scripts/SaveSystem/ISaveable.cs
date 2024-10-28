using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    GameDataDefinition GetGameDataID();

    void RegisterSaveData()
    {
        DataManager.Instance.RegisterSaveData(this);
    }

    void UnRegisterSaveData()
    {
        DataManager.Instance.UnRegisterSaveData(this);
    }

    void GetSaveData(GameData data);
    void LoadData(GameData data);
}
