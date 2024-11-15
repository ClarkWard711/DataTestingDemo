using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodFight"), fileName = ("BloodFight"))]
public class BloodFight : BloodistSkill
{
	public BloodFight()
	{
		SpCost = 15;
		bloodistSkillKind = BloodistSkillKind.other;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodFight(SpCost, bloodistSkillKind));
	}

}
