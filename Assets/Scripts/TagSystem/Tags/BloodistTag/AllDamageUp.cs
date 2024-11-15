using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllDamageUp : Tag
{
	public AllDamageUp()
	{
		TagName = "SoloTagDefense";
		TagKind = Kind.turnLessen;
		TurnAdd = 1;
		TurnLast = 1;
		Effect = effect.good;
		BuffTarget = target.self;
		Impact = impactOnMultiplier.AllDeal;
		Multiplier = 0.9f;
	}
}
