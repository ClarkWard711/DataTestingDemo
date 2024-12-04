using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/Launch"), fileName = ("Launch"))]
public class Launch : Skill
{
	public Launch()
	{
		SpCost = 8;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.launch(SpCost));
	}
}
