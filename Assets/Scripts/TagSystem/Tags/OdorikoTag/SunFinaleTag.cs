using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFinaleTag : OdorikoTag
{
	public bool isCharged = false;
	public GameObject AtkUnit, DfsUnit;
	public SunFinaleTag()
	{
		odoTagKind = OdoTagKind.Sun;
		TagName = "SunFinale";
		TagKind = Kind.accumulable;
		quantity = 1;
		Effect = effect.bad;
		BuffTarget = target.enemy;
		Impact = impactOnMultiplier.AllDeal;
		conditionMultiplier = 0.8f;
	}

	public override void OnTurnEndCallback()
	{
		if (DfsUnit != null)
		{
			int physicalDamage, soulDamage;
			physicalDamage = Mathf.CeilToInt(quantity * conditionMultiplier * BattleSetting.Instance.DamageCountingByUnit(AtkUnit, DfsUnit, AttackType.Physical));
			soulDamage = Mathf.CeilToInt(quantity * conditionMultiplier * BattleSetting.Instance.DamageCountingByUnit(AtkUnit, DfsUnit, AttackType.Soul));
			Debug.Log("SunFinaleActivating");
			int containMoonCount = 0;

			foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				if (enemy.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
				{
					containMoonCount++;
				}
			}

			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				if (player.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
				{
					containMoonCount++;
				}
			}

			containMoonCount = Mathf.RoundToInt(containMoonCount / 2);

			if (isCharged)
			{
				BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.MethodActivateDelay(() => BattleSetting.Instance.DealDamageWithNoCallBack(physicalDamage, AtkUnit, DfsUnit, AttackType.Physical, false), 0.1f, containMoonCount));
				BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.MethodActivateDelay(() => BattleSetting.Instance.DealDamageWithNoCallBack(soulDamage, AtkUnit, DfsUnit, AttackType.Soul, false), 0.1f, containMoonCount));
			}
			else
			{
				BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.MethodActivateDelay(() => BattleSetting.Instance.DealDamageWithNoCallBack(physicalDamage, AtkUnit, DfsUnit, AttackType.Physical, false), 0.1f, containMoonCount));
			}

			quantity = 0;
		}
		//BattleSetting.Instance.CurrentEndTurnUnit.GetComponent<GivingData>().tagList.Remove(this);
	}
}
