using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodStab"), fileName = ("BloodStab"))]
public class BloodStab : BloodistSkill
{
	public BloodStab()
	{
		SpCost = 12;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodStab(SpCost, bloodistSkillKind));
	}
}
