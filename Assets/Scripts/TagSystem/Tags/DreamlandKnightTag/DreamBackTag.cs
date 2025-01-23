using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DreamBackTag : Tag
{
	public GameObject unit;
	public DreamBackTag()
	{
		TagName = "DreamBackTag";
		TagKind = Kind.turnLessen;
		Effect = effect.bad;
		TurnAdd = 3;
		TurnLast = 3;
		BuffTarget = target.self;
	}

	public override void OnTurnEndCallback()
	{
		if (TurnLast == 1)
		{
			if (unit.GetComponent<DreamlandKnightHolder>().DreamCount > 6)
			{
				unit.GetComponent<DreamlandKnightHolder>().DreamCount -= 6;
			}
			else
			{
				unit.GetComponent<DreamlandKnightHolder>().DreamCount = 0;
			}
		}
	}
}
