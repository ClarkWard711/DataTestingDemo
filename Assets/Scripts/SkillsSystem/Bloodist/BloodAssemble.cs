using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAssemble : BloodistSkill
{
	public BloodAssemble()
	{
		SpCost = 6;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodAssemble(SpCost, bloodistSkillKind));
	}
}
