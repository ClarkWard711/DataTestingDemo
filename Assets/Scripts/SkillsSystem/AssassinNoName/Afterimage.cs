using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/Afterimage"), fileName = ("Afterimage"))]
public class Afterimage : Skill
{
	public Afterimage()
	{
		SpCost = 18;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.afterimage(SpCost));
	}
}
