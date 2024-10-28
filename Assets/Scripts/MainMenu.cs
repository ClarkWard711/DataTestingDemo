using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AssetReference newGameScene;
    [SerializeField] bool debugMode = true;
    bool enter = true;
    public void StartGame()
    {
        //todo: disable all player inputs
        if (debugMode)
        {
            enter = false;
            SceneLoader.LoadAddressableScene(SceneLoader.DebugRoomSceneKey);
        }
        if(enter)
        {
            enter = false;
            SceneLoader.LoadAddressableScene(newGameScene);

        }
    }
}
