using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/ShadowChange"), fileName = ("ShadowChange"))]
public class ShadowChange : Skill
{
	public ShadowChange()
	{
		SpCost = 18;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
		AssassinNoNameHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.shadowChange(SpCost));
	}
}
