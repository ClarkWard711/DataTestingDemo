using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
[DefaultExecutionOrder(-100)]
public class PlayerSaveController : MonoBehaviour, ISaveable
{
	public static PlayerSaveController Instance;
	public PlayerSaveData playerSaveData;
	public PartyMember AllJobs;
	public PartyMember playerParty;
	void Awake()
	{
		Instance = this;
		playerSaveData = new PlayerSaveData();
		playerSaveData.jobUnlockState.Add(0, true);
		playerSaveData.jobUnlockState.Add(1, true);
		playerSaveData.jobUnlockState.Add(2, true);
		for (int i = 0; i < 3; i++)
		{
			List<bool> lockedState = new List<bool>();
			for (int j = 0; j < AllJobs.CharacterList[i].JobPrefab.GetComponent<JobSkillHolder>().JobSkill.skillList.Count; j++)
			{
				lockedState.Add(true);
			}
			playerSaveData.skillUnlockState.Add(i, lockedState);
		}

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
