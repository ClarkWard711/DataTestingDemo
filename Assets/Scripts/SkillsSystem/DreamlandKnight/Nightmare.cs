using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/Nightmare"), fileName = ("Nightmare"))]
public class Nightmare : Skill
{
    public Nightmare()
	{
		SpCost = 15;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.nightmare(SpCost));
    }
}
