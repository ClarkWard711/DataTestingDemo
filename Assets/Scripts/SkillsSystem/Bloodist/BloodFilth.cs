using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodFilth"), fileName = ("BloodFilth"))]
public class BloodFilth : BloodistSkill
{
	public BloodFilth()
	{
		SpCost = 10;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodFilth(SpCost, bloodistSkillKind));
	}
}
