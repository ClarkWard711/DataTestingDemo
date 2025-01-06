using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/Oneiromancy"), fileName = ("Oneiromancy"))]
public class Oneiromancy : Skill
{
    public Oneiromancy()
	{
		SpCost = 8;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
        DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.oneiromancy(SpCost));
    }
}
