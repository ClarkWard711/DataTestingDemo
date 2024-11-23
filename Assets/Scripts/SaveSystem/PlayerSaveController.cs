using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
[DefaultExecutionOrder(-100)]
public class PlayerSaveController : MonoBehaviour, ISaveable
{
	public static PlayerSaveController Instance;
	public PlayerSaveData playerSaveData;
	void Awake()
	{
		Instance = this;
		playerSaveData = new PlayerSaveData();
	}
	public void OnEnable()
	{
		ISaveable Saveable;
		Saveable = this;
		Saveable.RegisterSaveData();
	}
	public void OnDisable()
	{
		ISaveable Saveable;
		Saveable = this;
		Saveable.UnRegisterSaveData();
	}
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
}
