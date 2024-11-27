using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weak : Tag
{
	public Weak()
	{
		TagName = "Weak";
		Impact = impactOnMultiplier.AllDeal;
		Multiplier = 0.7f;
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.enemy;
		Effect = effect.bad;
		TurnLast = 1;
		spd = -10;
	}
}
