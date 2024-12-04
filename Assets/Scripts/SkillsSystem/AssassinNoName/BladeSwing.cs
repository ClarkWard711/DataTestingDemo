using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/BladeSwing"), fileName = ("BladeSwing"))]
public class BladeSwing : Skill
{
	public BladeSwing()
	{
		SpCost = 20;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.bladeSwing(SpCost));
	}
}
