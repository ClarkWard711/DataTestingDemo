using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/ShadowFollow"), fileName = ("ShadowFollow"))]
public class ShadowFollow : Skill
{
	public ShadowFollow()
	{
		SpCost = 6;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.shadowFollow(SpCost));
	}
}
