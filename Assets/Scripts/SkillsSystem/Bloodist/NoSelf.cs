using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSelf : BloodistSkill
{
	public NoSelf()
	{
		SpCost = 8;
		TurnCooldown = 3;
		bloodistSkillKind = BloodistSkillKind.other;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.noSelf(SpCost, bloodistSkillKind));
	}

}
