using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/IllusionFear"), fileName = ("IllusionFear"))]
public class IllusionFear : Skill
{
	public IllusionFear()
	{
		SpCost = 16;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.illusionFear(SpCost));
	}
}
