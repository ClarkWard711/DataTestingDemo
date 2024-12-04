using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/Sneak"), fileName = ("Sneak"))]
public class Sneak : Skill
{
	public Sneak()
	{
		SpCost = 16;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.sneak(SpCost));
	}
}
