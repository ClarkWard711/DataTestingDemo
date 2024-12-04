using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/FalseRetreat"), fileName = ("FalseRetreat"))]
public class FalseRetreat : Skill
{
	public FalseRetreat()
	{
		SpCost = 8;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.falseRetreat(SpCost));
	}
}
