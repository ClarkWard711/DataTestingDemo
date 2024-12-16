using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : Tag
{
	public GameObject unit;
	public Burn()
	{
		TagName = "Burn";
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.all;
		Effect = effect.bad;
		TurnLast = 1;
	}

	public override void OnTurnEndCallback()
	{
		BattleSetting.Instance.DealDamageWithNoCallBack(50, unit, unit, AttackType.Physical, false);
	}
}
