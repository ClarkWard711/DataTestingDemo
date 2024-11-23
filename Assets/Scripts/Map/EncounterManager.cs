using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edgar.Unity;
using UnityEngine.AddressableAssets;
using JetBrains.Annotations;
using Edgar.Unity.Examples;

public class EncounterManager : MonoBehaviour
{
	public static EncounterManager instance;
	public GameObject generator;
	public int seed;
	[SerializeField] AssetReference battleScene;
	public GameObject level;
	public GameObject player;
	public int currentSteps = 0;
	public int stepsToEncounter = 15;
	public bool isLocated;
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
		player = level.GetComponentInChildren<SimplePlayer2D>().gameObject;

	}

	public void Update()
	{
		if (GameObject.FindGameObjectWithTag("Player") != null && !isLocated)
		{
			isLocated = true;
			if (PlayerSaveController.Instance.playerSaveData.playerPosition != Vector3.zero)
			{
				Debug.Log(PlayerSaveController.Instance.playerSaveData.playerPosition);
				RestoreState();
			}
			if (PlayerSaveController.Instance.playerSaveData.stepsToEncounter != 0)
			{
				stepsToEncounter = PlayerSaveController.Instance.playerSaveData.stepsToEncounter;
			}
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

