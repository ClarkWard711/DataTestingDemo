using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protect : Tag
{
	public Protect()
	{
		TagName = "Protect";
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.self;
		Effect = effect.neutral;
		TurnLast = 1;
	}
}
