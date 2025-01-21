using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/UnwillingToWake"), fileName = ("UnwillingToWake"))]
public class UnwillingToWake : Skill
{
	public UnwillingToWake()
	{
		SpCost = 12;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.unwillingToWake(SpCost));
	}
}
