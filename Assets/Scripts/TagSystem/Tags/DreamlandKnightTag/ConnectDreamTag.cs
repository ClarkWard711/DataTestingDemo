using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectDreamTag : Tag
{
	public GameObject unit;
	public ConnectDreamTag()
	{
		TagName = "ConnectDreamTag";
		TagKind = Kind.turnLessen;
		Effect = effect.good;
		TurnAdd = 3;
		TurnLast = 3;
		BuffTarget = target.self;
		OnHit += DealExtraDamage;
	}

	public void DealExtraDamage()
	{
		if (BattleSetting.Instance.isNormalAttack)
		{
			int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, unit, AttackType.Physical);
			damage = Mathf.CeilToInt(damage * (1 + 0.1f * BattleSetting.Instance.CurrentActUnit.GetComponent<DreamlandKnightHolder>().DreamCount));
			BattleSetting.Instance.DealDamageWithNoCallBack(damage, BattleSetting.Instance.CurrentActUnit, unit, AttackType.Physical, false);
		}
	}
}
