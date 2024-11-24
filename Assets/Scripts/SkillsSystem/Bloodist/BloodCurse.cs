using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodCurse"), fileName = ("BloodCurse"))]
public class BloodCurse : BloodistSkill
{
	public BloodCurse()
	{
		SpCost = 12;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodCurse(SpCost, bloodistSkillKind));
	}
}
