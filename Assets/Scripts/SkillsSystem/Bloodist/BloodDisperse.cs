using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/BloodDisperse"), fileName = ("BloodDisperse"))]
public class BloodDisperse : BloodistSkill
{
	public BloodDisperse()
	{
		SpCost = 5;
		bloodistSkillKind = BloodistSkillKind.Blood;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		//BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodDisperse(SpCost, bloodistSkillKind));
	}
}
