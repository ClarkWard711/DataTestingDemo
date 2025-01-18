using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/WakingIssue"), fileName = ("WakingIssue"))]
public class WakingIssue : Skill
{
	public WakingIssue()
	{
		SpCost = 18;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.wakingIssue(SpCost));
	}
}
