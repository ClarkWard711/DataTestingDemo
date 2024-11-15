using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/Solo"), fileName = ("Solo"))]
public class Solo : BloodistSkill
{
	public Solo()
	{
		SpCost = 18;
		bloodistSkillKind = BloodistSkillKind.other;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.solo(SpCost, bloodistSkillKind));
	}

}
