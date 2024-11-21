using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodRain"), fileName = ("BloodRain"))]
public class BloodRain : BloodistSkill
{
	public BloodRain()
	{
		SpCost = 25;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodRain(SpCost, bloodistSkillKind));
	}
}
