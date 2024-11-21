using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodAlly"), fileName = ("BloodAlly"))]
public class BloodAlly : BloodistSkill
{
	public BloodAlly()
	{
		SpCost = 24;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.isWaitForPlayerToChooseDead = true;
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodAlly(SpCost, bloodistSkillKind));
	}
}
