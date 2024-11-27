using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodInfect"), fileName = ("BloodInfect"))]
public class BloodInfect : BloodistSkill
{
	public BloodInfect()
	{
		SpCost = 15;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodInfect(SpCost, bloodistSkillKind));
	}
}
