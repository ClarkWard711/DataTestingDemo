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

		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
		{
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				var tag = SoulAtkUp.CreateInstance<SoulAtkUp>();
				tag.TurnAdd = DreamCount;
				tag.TurnLast = DreamCount;
				player.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
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
		DreamCount++;
		foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
		{
			var tag = PhysicalDfsUp.CreateInstance<PhysicalDfsUp>();
			tag.TurnAdd = 2;
			tag.TurnLast = 2;
			if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
			{
				tag.TurnAdd++;
				tag.TurnLast++;
			}
			player.GetComponent<GivingData>().AddTagToCharacter(tag);
		}
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
		{
			DreamCount++;
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("梦的庇护"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator nightmare(int SpCost)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		DreamCount++;
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);

		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
		{
			damage = Mathf.CeilToInt(damage * 2f);
			DreamCount++;
		}
		else
		{
			damage = Mathf.CeilToInt(damage * 1.2f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		StartCoroutine(BattleSetting.Instance.OnDealDamage());
		var tag = Fear.CreateInstance<Fear>();
		tag.TurnAdd++;
		tag.TurnLast++;
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("噩梦缠绕"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator dreamSurround(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		DreamSurroundTag tag = DreamSurroundTag.CreateInstance<DreamSurroundTag>();
		tag.unit = gameObject;
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
		{
			DreamCount += 2;
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("梦境护身"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator dreamExtend(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;

		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
		{
			DreamExtendAdvanced tag = DreamExtendAdvanced.CreateInstance<DreamExtendAdvanced>();
			tag.unit = gameObject;
			gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		}
		else
		{
			DreamExtendTag tag = DreamExtendTag.CreateInstance<DreamExtendTag>();
			tag.unit = gameObject;
			gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("梦的延伸"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator wakingIssue(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		for (int i = 0; i < DreamCount; i++)
		{
			var randomEnemy = BattleSetting.Instance.RemainingEnemyUnits[Random.Range(0, BattleSetting.Instance.RemainingEnemyUnits.Length)];
			int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, randomEnemy, AttackType.Physical);

			if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
			{
				damage = Mathf.CeilToInt(damage * 0.6f);
			}
			else
			{
				damage = Mathf.CeilToInt(damage * 0.4f);
			}
			BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, randomEnemy, AttackType.Physical, false);
			yield return new WaitForSeconds(0.25f);
		}
		DreamCount = 0;
		StartCoroutine(BattleSetting.Instance.OnDealDamage());
		StartCoroutine(BattleSetting.Instance.ShowActionText("起床气"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator unwillingToWake(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
		{

		}
		else
		{
			if (DreamCount >= 0)
			{
				DreamCount--;
			}
		}
		DreamCount *= 2;
		StartCoroutine(BattleSetting.Instance.ShowActionText("不愿苏醒"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator allDream(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is Charging))
		{

		}
		else
		{

		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("共梦"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
	#endregion
}
