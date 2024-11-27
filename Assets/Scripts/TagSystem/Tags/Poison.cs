using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : Tag
{
	public float percentage;
	public Poison()
	{
		TagName = "Poison";
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.all;
		Effect = effect.bad;
		TurnLast = 1;
	}
}
