using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/AllDream"), fileName = ("AllDream"))]
public class AllDream : Skill
{
	public AllDream()
	{
		SpCost = 10;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.allDream(SpCost));
	}
}
