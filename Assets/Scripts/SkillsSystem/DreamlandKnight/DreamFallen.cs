using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/DreamFallen"), fileName = ("DreamFallen"))]
public class DreamFallen : Skill
{
    public DreamFallen()
	{
		SpCost = 5;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
        DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.dreamFallen(SpCost));
    }
}
