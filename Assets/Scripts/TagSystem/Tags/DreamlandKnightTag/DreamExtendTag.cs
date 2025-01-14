using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamExtendTag : Tag
{
	public GameObject unit;
	public DreamExtendTag()
	{
		TagName = "DreamExtendTag";
		TagKind = Kind.turnLessen;
		Effect = effect.good;
		TurnAdd = 3;
		TurnLast = 3;
		BuffTarget = target.self;
	}

	public override void OnTurnEndCallback()
	{
		unit.GetComponent<DreamlandKnightHolder>().DreamCount += 2;
	}
}
