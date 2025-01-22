using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/InTheNameOfDream"), fileName = ("InTheNameOfDream"))]
public class InTheNameOfDream : Skill
{
	public InTheNameOfDream()
	{
		SpCost = 8;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.inTheNameOfDream(SpCost));
	}
}
