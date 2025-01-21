using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDreamTag : Tag
{
	public AllDreamTag()
	{
		TagName = "AllDreamTag";
		TagKind = Kind.turnLessen;
		TurnAdd = 3;
		TurnLast = 3;
		Effect = effect.good;
		BuffTarget = target.ally;
		BeforeHit += NormalAttackCheck;
	}

	public void NormalAttackCheck()
	{
		if (BattleSetting.Instance.isNormalAttack)
		{
			BattleSetting.Instance.damageCache = Mathf.CeilToInt(1 + 0.1f * DreamlandKnightHolder.Instance.DreamCount);
		}
	}
}
