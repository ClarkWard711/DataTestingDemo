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
		if (playerSaveData.jobStatsState != null)
		{
			foreach (var player in playerParty.CharacterList)
			{
				if (player != null)
				{
					JobStatsData job = new JobStatsData();
					job.currentExp = player.currentExp;
					job.currentHp = player.currentHP;
					job.specialID = player.SpecialID;
					job.currentSp = player.currentSP;
					job.level = player.JobLevel;
					for (int i = 0; i < 4; i++)
					{
						job.skillsID.Add(0);
						job.skillsID[i] = player.SkillsID[i];
					}
					PlayerSaveController.Instance.playerSaveData.jobStatsState[player.JobID] = job;
				}
			}
		}
		data.playerSaveData = playerSaveData;
	}

	public void LoadData(GameData data)
	{
		playerSaveData = data.playerSaveData;
		if (playerSaveData.jobStatsState != null)
		{
			for (int i = 0; i < 6; i++)
			{
				playerParty.CharacterList[i] = null;
			}
			foreach (var Key in playerSaveData.jobStatsState.Keys)
			{
				int i;
				playerSaveData.positionID.TryGetValue(Key, out i);
				JobStatsData job;
				playerSaveData.jobStatsState.TryGetValue(Key, out job);
				playerParty.CharacterList[i] = AllJobs.CharacterList[Key];
				AllJobs.CharacterList[Key].currentExp = job.currentExp;
				AllJobs.CharacterList[Key].JobLevel = job.level;
				AllJobs.CharacterList[Key].currentHP = job.currentHp;
				AllJobs.CharacterList[Key].currentSP = job.currentSp;
				AllJobs.CharacterList[Key].SpecialID = job.specialID;
				for (int j = 0; j < 4; j++)
				{
					AllJobs.CharacterList[Key].SkillsID[j] = job.skillsID[j];
				}
			}
		}
	}
}
