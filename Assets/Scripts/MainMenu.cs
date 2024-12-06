using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainMenu : MonoBehaviour
{
	[SerializeField] AssetReference newGameScene;
	[SerializeField] AssetReference chooseScene;
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
		if (enter)
		{
			enter = false;
			DataManager.Instance.Load();
			if (PlayerSaveController.Instance.playerSaveData.seed == 0 || PlayerSaveController.Instance.playerSaveData.isNewGame)
			{
				//PlayerSaveController.Instance.playerSaveData.isNewGame = false;
				PlayerSaveController.Instance.playerSaveData.seed = Random.Range(10000000, 100000000);
				PlayerSaveController.Instance.playerSaveData.jobStatsState.Clear();
				PlayerSaveController.Instance.playerSaveData.playerPosition = Vector3.zero;
				DataManager.Instance.Save();
				SceneLoader.LoadAddressableScene(chooseScene);
			}
			else
			{
				DataManager.Instance.Save();
				SceneLoader.LoadAddressableScene(newGameScene);
			}

		}
	}
}
