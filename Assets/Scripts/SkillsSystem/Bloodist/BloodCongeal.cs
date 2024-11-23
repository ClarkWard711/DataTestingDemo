using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodCongeal"), fileName = ("BloodCongeal"))]
public class BloodCongeal : BloodistSkill
{
	public BloodCongeal()
	{
		SpCost = 14;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodCongeal(SpCost, bloodistSkillKind));
	}
}
