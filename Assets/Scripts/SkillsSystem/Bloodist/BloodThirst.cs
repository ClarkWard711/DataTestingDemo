using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodThirst"), fileName = ("BloodThirst"))]
public class BloodThirst : BloodistSkill
{
	public BloodThirst()
	{
		SpCost = 17;
		bloodistSkillKind = BloodistSkillKind.other;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodThirst(SpCost, bloodistSkillKind));
	}
}
