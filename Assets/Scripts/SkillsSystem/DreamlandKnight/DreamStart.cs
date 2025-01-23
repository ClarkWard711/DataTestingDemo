using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/DreamStart"), fileName = ("DreamStart"))]
public class DreamStart : Skill
{
	public DreamStart()
	{
		SpCost = 8;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.dreamStart(SpCost));
	}
}
