using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class EncounterManager : MonoBehaviour
{
    public static EncounterManager instance;

    [SerializeField] AssetReference battleScene;
    
    public int currentSteps = 0;
    public int stepsToEncounter = 15;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetSteps()
    {
        currentSteps = 0;
        stepsToEncounter += 5;
    }
    
    public void TriggerBattle()
    {
        Debug.Log("Battle Start！");
        SceneLoader.LoadAddressableScene(battleScene);
        ResetSteps();
    }
}

