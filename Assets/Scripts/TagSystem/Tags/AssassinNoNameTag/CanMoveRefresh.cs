using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveRefresh : Tag
{
	public CanMoveRefresh()
	{
		TagName = "CanMoveRefresh";
		TagKind = Kind.special;
		BuffTarget = target.self;
		Effect = effect.neutral;
	}

	public override void OnTurnStartCallback()
	{
		AssassinNoNameHolder.Instance.canMoveCount = AssassinNoNameHolder.Instance.canMoveCountLimit;
	}
}
