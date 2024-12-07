using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EnemyDialog : MonoBehaviour
{
	public GameObject enemyDialog;

	[SerializeField] AssetReference battleScene;
	bool enter;
	bool enterDialog;
	private void Awake()
	{
		//GameObject.FindGameObjectWithTag("MainCamera").SetActive(true);
		enter = true;
	}

	private void FixedUpdate()
	{
		/*if (Input.GetMouseButton(0))
		{
			DataInputs.Instance.playerCurrentLevel = 2;
			DataInputs.Instance.UpdatePlayerStats(DataInputs.Instance.playerCurrentLevel);
		}*/

		if (Input.GetKeyDown(KeyCode.F) && enter && enterDialog)
		{
			enter = false;
			if (PlayerSaveController.Instance.playerSaveData.seed == 0 || PlayerSaveController.Instance.playerSaveData.isNewGame)
			{
				PlayerSaveController.Instance.playerSaveData.playerPosition = Vector3.zero;
				PlayerSaveController.Instance.playerSaveData.seed = Random.Range(10000000, 100000000);
				PlayerSaveController.Instance.playerSaveData.isNewGame = false;
				PlayerSaveController.Instance.playerSaveData.stepsToEncounter = 15;
			}
			DataManager.Instance.Save();
			//GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
			SceneLoader.LoadAddressableScene(battleScene);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			enemyDialog.SetActive(true);
			enterDialog = true;
		}


	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			enemyDialog.SetActive(false);
			enterDialog = false;
		}
	}
}
