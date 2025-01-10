using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/DreamSurround"), fileName = ("DreamSurround"))]
public class DreamSurround : Skill
{
	public DreamSurround()
	{
		SpCost = 12;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.dreamSurround(SpCost));
	}
}
