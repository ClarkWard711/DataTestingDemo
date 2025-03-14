using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/Collaborate"), fileName = ("Collaborate"))]
public class Collaborate : Skill
{
	public Collaborate()
	{
		SpCost = 8;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.collaborate(SpCost));
	}
}
