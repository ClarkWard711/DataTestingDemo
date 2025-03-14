using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloTag : Tag
{
	public SoloTag()
	{
		TagName = "SoloTag";
		TagKind = Kind.turnLessen;
		TurnAdd = 2;
		TurnLast = 2;
		Effect = effect.good;
		BuffTarget = target.self;
		Impact = impactOnMultiplier.PhysicalDeal;
		Multiplier = 1.1f;
		cri = 0;
	}
}
