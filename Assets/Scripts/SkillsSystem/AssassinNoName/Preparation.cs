using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/Preparation"), fileName = ("Preparation"))]
public class Preparation : Skill
{
	public Preparation()
	{
		SpCost = 14;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.preparation(SpCost));
	}
}
