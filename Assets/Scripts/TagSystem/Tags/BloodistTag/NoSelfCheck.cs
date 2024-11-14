using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSelfCheck : Tag
{
	public NoSelfCheck()
	{
		TagName = "NoSelfCheck";
		TagKind = Kind.turnLessen;
		BuffTarget = target.self;
		Effect = effect.neutral;
		TurnAdd = 2;
		TurnLast = 2;
	}

	public override void OnTurnEndCallback()
	{
		if (TurnLast == 1)
		{
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				BloodAddict bloodTag = (BloodAddict)player.GetComponent<GivingData>().tagList.Find(tag => tag.TagName == "BloodAddict");
				bloodTag.isNoSelf = false;
			}
		}
	}
}
