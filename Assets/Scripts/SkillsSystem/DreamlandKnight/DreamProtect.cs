using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/DreamProtect"), fileName = ("DreamProtect"))]
public class DreamProtect : Skill
{
    public DreamProtect()
	{
		SpCost = 7;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
        DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.dreamProtect(SpCost));
    }
}
