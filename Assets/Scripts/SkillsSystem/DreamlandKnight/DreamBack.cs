using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/DreamBack"), fileName = ("DreamBack"))]
public class DreamBack : Skill
{
	public DreamBack()
	{
		SpCost = 15;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.dreamBack(SpCost));
	}
}
