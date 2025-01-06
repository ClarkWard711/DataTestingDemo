using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DreamlandKnightHolder : JobSkillHolder
{
	public static DreamlandKnightHolder Instance;
	public int DreamCount;
	public int DreamFallenCount = 1;
	public override void Awake()
	{
		base.Awake();
		if (Instance == null)
		{
			Instance = this;
		}
	}
	#region  基础
	public IEnumerator dreamFallen(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		DreamCount += DreamFallenCount;
		DreamFallenCount++;
		StartCoroutine(BattleSetting.Instance.ShowActionText("坠梦"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator oneiromancy(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
		{
			var tag = PhysicalAtkUp.CreateInstance<PhysicalAtkUp>();
			tag.TurnAdd = DreamCount;
			tag.TurnLast = DreamCount;
			player.GetComponent<GivingData>().AddTagToCharacter(tag);
		}
		DreamCount = 0;
		StartCoroutine(BattleSetting.Instance.ShowActionText("解梦"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
	#endregion

	#region Advanced
	public IEnumerator dreamProtect(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
		{
			var tag = PhysicalDfsUp.CreateInstance<PhysicalDfsUp>();
			tag.TurnAdd = 2;
			tag.TurnLast = 2;
			player.GetComponent<GivingData>().AddTagToCharacter(tag);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("梦的庇护"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
	#endregion
}
