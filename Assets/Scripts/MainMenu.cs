using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainMenu : MonoBehaviour
{
	[SerializeField] AssetReference outerScene;
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

				PlayerSaveController.Instance.playerSaveData.jobStatsState.Clear();

				DataManager.Instance.Save();
				SceneLoader.LoadAddressableScene(chooseScene);
			}
			else
			{
				DataManager.Instance.Load();
				SceneLoader.LoadAddressableScene(outerScene);
			}

		}
	}
}
