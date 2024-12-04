using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Tag
{
	public float percentage;
	public GameObject unit;
	public Poison()
	{
		TagName = "Poison";
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.all;
		Effect = effect.bad;
		TurnLast = 1;
	}

	public override void OnTurnEndCallback()
	{
		if (TurnLast == 1)
		{
			int damage = Mathf.CeilToInt(percentage * unit.GetComponent<GivingData>().maxHP);
			BattleSetting.Instance.DealDamageWithNoCallBack(damage, unit, unit, AttackType.Physical, false);
		}
	}
}
