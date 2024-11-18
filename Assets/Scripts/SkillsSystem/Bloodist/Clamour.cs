using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/Clamour"), fileName = ("Clamour"))]
public class Clamour : BloodistSkill
{
	public Clamour()
	{
		SpCost = 15;
		bloodistSkillKind = BloodistSkillKind.other;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.clamour(SpCost, bloodistSkillKind));
	}
}

