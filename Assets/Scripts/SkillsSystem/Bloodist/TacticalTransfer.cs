using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/BloodistSkill/TacticalTransfer"), fileName = ("TacticalTransfer"))]
public class TacticalTransfer : BloodistSkill
{
	public TacticalTransfer()
	{
		SpCost = 6;
		bloodistSkillKind = BloodistSkillKind.other;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.tacticalTransfer(SpCost, bloodistSkillKind));
	}
}
