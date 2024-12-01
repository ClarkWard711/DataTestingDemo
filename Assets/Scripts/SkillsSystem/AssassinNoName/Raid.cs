using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/AssassinNoNameSkill/Raid"), fileName = ("Raid"))]
public class Raid : Skill
{
	public Raid()
	{
		SpCost = 6;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		OdorikoHolder.Instance.CoroutineStart(AssassinNoNameHolder.Instance.raid(SpCost));
	}
}
