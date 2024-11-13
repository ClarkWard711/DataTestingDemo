using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloTagDefense : Tag
{
	public SoloTagDefense()
	{
		TagName = "SoloTagDefense";
		TagKind = Kind.turnLessen;
		TurnAdd = 2;
		TurnLast = 2;
		Effect = effect.good;
		BuffTarget = target.self;
		Impact = impactOnMultiplier.AllTake;
		Multiplier = 0.9f;
		cri = 0;
	}
}
