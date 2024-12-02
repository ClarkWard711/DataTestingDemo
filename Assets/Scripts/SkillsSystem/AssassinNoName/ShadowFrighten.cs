using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/ShadowFrighten"), fileName = ("ShadowFrighten"))]
public class ShadowFrighten : Skill
{
	public ShadowFrighten()
	{
		SpCost = 8;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		OdorikoHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.shadowFrighten(SpCost));
	}
}
