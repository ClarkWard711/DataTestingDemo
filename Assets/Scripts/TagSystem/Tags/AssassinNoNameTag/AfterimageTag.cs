using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageTag : Tag
{
	int turnCount;
	public float mult = 0.6f;
	public AfterimageTag()
	{
		TagName = "AfterimageTag";
		TurnAdd = 3;
		TagKind = Kind.turnLessen;
		BuffTarget = target.self;
		Effect = effect.good;
		Impact = impactOnMultiplier.AllTake;
		TurnLast = 3;
		spd = 0;
		BeforeBeingHit += Main;
	}

	public override void OnTurnEndCallback()
	{
		if (turnCount == 0)
		{
			turnCount++;
		}
	}

	public void Main()
	{
		if (turnCount > 0)
		{
			BattleSetting.Instance.damageCache = Mathf.CeilToInt(mult * BattleSetting.Instance.damageCache);
			turnCount--;
		}
	}
}
