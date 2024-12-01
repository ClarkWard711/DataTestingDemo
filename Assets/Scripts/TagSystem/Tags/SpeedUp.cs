using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : Tag
{
	public SpeedUp()
	{
		TagName = "SpeedUp";
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.all;
		Effect = effect.good;
		TurnLast = 1;
		spd = 0;
	}
}
