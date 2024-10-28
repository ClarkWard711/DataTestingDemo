using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public string sceneToSave;
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();
    public Dictionary<string, int> intSavedData = new Dictionary<string, int>();
}
