using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class AssassinNoNameHolder : JobSkillHolder
{
	public static AssassinNoNameHolder Instance;

	public int canMoveCount = 1;
	public int canMoveCountLimit = 1;
	public override void Awake()
	{
		base.Awake();
		if (Instance == null)
		{
			Instance = this;
		}
	}
	private void Start()
	{
		var tag = CanMoveRefresh.CreateInstance<CanMoveRefresh>();
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
	}
	public override void AddSkillToButton()
	{
		base.AddSkillToButton();
	}
	#region  基础
	public IEnumerator raid(int SpCost)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			damage = Mathf.CeilToInt(damage * 2f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		var tag = SpeedDown.CreateInstance<SpeedDown>();
		tag.spd = -Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().EnemyData.EnemyStatsList[BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().GetComponent<GivingData>().EnemyData.EnemyLevel - 1].speed * 0.1f);
		tag.TurnAdd++;
		tag.TurnLast++;
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("偷袭"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator collaborate(int SpCost)
	{
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			BattleSetting.Instance.canChangeAction = false;
			BattleSetting.Instance.isChooseFinished = false;
			BattleSetting.Instance.State = BattleState.Middle;
			BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				var tag = SpeedUp.CreateInstance<SpeedUp>();
				tag.TurnAdd++;
				tag.TurnLast++;
				tag.spd = Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f);
				player.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
		}
		else
		{
			BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
			yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
			BattleSetting.Instance.canChangeAction = false;
			BattleSetting.Instance.isChooseFinished = false;
			BattleSetting.Instance.State = BattleState.Middle;
			BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
			var tag = SpeedUp.CreateInstance<SpeedUp>();
			tag.spd = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().jobData.JobStatsList[BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f);
			tag.TurnAdd++;
			tag.TurnLast++;
			BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			var tag0 = SpeedUp.CreateInstance<SpeedUp>();
			tag0.TurnAdd++;
			tag0.TurnLast++;
			tag0.spd = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().jobData.JobStatsList[gameObject.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f);
			gameObject.GetComponent<GivingData>().AddTagToCharacter(tag0);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("协同"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
	#endregion
	public override void ActionEndCallback()
	{
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote") && canMoveCount > 0)
		{
			canMoveCount--;
			BattleSetting.Instance.BattleUnitsList.Insert(0, gameObject);
			BattleSetting.Instance.BattleUnitsListToBeLaunched.RemoveAt(BattleSetting.Instance.BattleUnitsListToBeLaunched.FindIndex(obj => obj == gameObject));
		}
		base.ActionEndCallback();
	}
}
