using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class FinishManager : MonoBehaviour
{
	public GameObject FinPanel;
	public Text StateText;
	public Text ExpNum;
	public Text Level;
	public Text RemainLevel;
	public int gainedExp;
	public int remainExp;
	public int LevelExp;
	public GameObject WinPanel;
	public GameObject LosePanel;
	[SerializeField] AssetReference mapScene;
	[SerializeField] AssetReference mainMenuScene;
	public bool state;
	public void ShowFinPanel(bool isWin)
	{
		FinPanel.SetActive(true);
		state = isWin;
		if (isWin)
		{
			StateText.text = "胜利";
			WinPanel.SetActive(true);
			foreach (var enemy in BattleSetting.Instance.EnemyPartyMember.EnemyDataList)
			{
				if (enemy != null)
				{
					gainedExp += enemy.EnemyStatsList[enemy.EnemyLevel - 1].exp;
				}
			}
			foreach (var player in BattleSetting.Instance.playerUnits)
			{
				//JobStatsData job;
				//PlayerSaveController.Instance.playerSaveData.jobStatsState.TryGetValue(player.JobID, out job);
				player.GetComponent<GivingData>().exp = gainedExp;
				if (gainedExp >= player.GetComponent<GivingData>().expPerlevel)
				{
					player.GetComponent<GivingData>().jobData.JobLevel++;
					player.GetComponent<GivingData>().exp = gainedExp - player.GetComponent<GivingData>().expPerlevel;
					player.GetComponent<GivingData>().currentHP = player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().jobData.JobLevel - 1].maxHP;
					player.GetComponent<GivingData>().currentSP = player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().jobData.JobLevel - 1].maxSP;
				}
				if (player.GetComponent<GivingData>().isDead)
				{
					player.GetComponent<GivingData>().isDead = false;
					player.GetComponent<GivingData>().currentHP = 1;
				}
				player.GetComponent<GivingData>().jobData.currentExp = player.GetComponent<GivingData>().exp;
				player.GetComponent<GivingData>().jobData.currentHP = player.GetComponent<GivingData>().currentHP;
				player.GetComponent<GivingData>().jobData.currentSP = player.GetComponent<GivingData>().currentSP;
			}
			remainExp = BattleSetting.Instance.playerUnits[0].GetComponent<GivingData>().exp;
			LevelExp = BattleSetting.Instance.playerUnits[0].GetComponent<GivingData>().jobData.JobStatsList[BattleSetting.Instance.playerUnits[0].GetComponent<GivingData>().jobData.JobLevel - 1].expPerLevel;
			RemainLevel.text = remainExp.ToString() + "/" + LevelExp.ToString();
			ExpNum.text = gainedExp.ToString();
			Level.text = BattleSetting.Instance.playerUnits[0].GetComponent<GivingData>().jobData.JobLevel.ToString();
			DataManager.Instance.Save();
		}
		else
		{
			LosePanel.SetActive(true);
			PlayerSaveController.Instance.playerSaveData.jobStatsState.Clear();
			PlayerSaveController.Instance.playerSaveData.isNewGame = true;
			PlayerSaveController.Instance.playerSaveData.seed = 0;
			PlayerSaveController.Instance.playerSaveData.playerPosition = Vector3.zero;
			StateText.text = "失败";

		}
	}

	public void BackToMap()
	{
		if (state)
		{
			DataManager.Instance.Save();
			SceneLoader.LoadAddressableScene(mapScene);
		}
		else
		{
			DataManager.Instance.Save();
			SceneLoader.LoadAddressableScene(mainMenuScene);
		}
	}
}
