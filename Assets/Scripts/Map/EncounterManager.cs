using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity;
using UnityEngine.AddressableAssets;

public class EncounterManager : MonoBehaviour
{
	public static EncounterManager instance;
	public GameObject generator;
	public int seed;
	[SerializeField] AssetReference battleScene;

	public int currentSteps = 0;
	public int stepsToEncounter = 15;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			//DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}

		DataManager.Instance.Load();
		if (PlayerSaveController.Instance.playerSaveData.seed != 0)
		{
			seed = PlayerSaveController.Instance.playerSaveData.seed;
			generator.GetComponent<DungeonGeneratorBaseGrid2D>().RandomGeneratorSeed = seed;
			generator.GetComponent<DungeonGeneratorBaseGrid2D>().Generate();
		}
	}


	public void ResetSteps()
	{
		currentSteps = 0;
		stepsToEncounter += 5;
	}
	public void SaveState(Transform playerTransform)
	{
		PlayerSaveController.Instance.playerSaveData.playerPosition = playerTransform.position;
		PlayerSaveController.Instance.playerSaveData.playerRotation = playerTransform.rotation;
	}

	public void RestoreState()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = PlayerSaveController.Instance.playerSaveData.playerPosition;
		player.transform.rotation = PlayerSaveController.Instance.playerSaveData.playerRotation;
	}
	public void TriggerBattle()
	{
		Debug.Log("Battle Start！");
		ResetSteps();
		SaveState(GameObject.FindGameObjectWithTag("Player").transform);
		Debug.Log(PlayerSaveController.Instance.playerSaveData.playerPosition);
		PlayerSaveController.Instance.playerSaveData.stepsToEncounter = stepsToEncounter;
		DataManager.Instance.Save();
		SceneLoader.LoadAddressableScene(battleScene);
	}
}

