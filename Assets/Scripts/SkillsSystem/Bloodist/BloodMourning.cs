using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodMourning"), fileName = ("BloodMourning"))]
public class BloodMourning : BloodistSkill
{
	public BloodMourning()
	{
		SpCost = 20;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodMourning(SpCost, bloodistSkillKind));
	}
}
