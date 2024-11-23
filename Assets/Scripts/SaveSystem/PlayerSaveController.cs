using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveController : MonoBehaviour, ISaveable
{
	public PlayerSaveData playerSaveData;
	public GameDataDefinition GetGameDataID()
	{
		return GetComponent<GameDataDefinition>();
	}

	public void GetSaveData(GameData data)
	{
		data.playerSaveData = playerSaveData;
	}

	public void LoadData(GameData data)
	{
		playerSaveData = data.playerSaveData;
	}
	
	public void SaveState(Transform playerTransform)
	{
		playerSaveData.playerPosition = playerTransform.position;
		playerSaveData.playerRotation = playerTransform.rotation;
	}
	
	public void RestoreState()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = playerSaveData.playerPosition;
		player.transform.rotation = playerSaveData.playerRotation;
	}
	
}
