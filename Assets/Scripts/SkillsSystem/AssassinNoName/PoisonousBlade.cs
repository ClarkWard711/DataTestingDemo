using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/PoisonousBlade"), fileName = ("PoisonousBlade"))]
public class PoisonousBlade : Skill
{
	public PoisonousBlade()
	{
		SpCost = 20;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.poisonousBlade(SpCost));
	}
}
