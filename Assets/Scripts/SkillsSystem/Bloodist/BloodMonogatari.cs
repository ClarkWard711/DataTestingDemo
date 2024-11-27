using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodMonogatari"), fileName = ("BloodMonogatari"))]
public class BloodMonogatari : BloodistSkill
{
	public BloodMonogatari()
	{
		SpCost = 20;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodMonogatari(SpCost, bloodistSkillKind));
	}
}
