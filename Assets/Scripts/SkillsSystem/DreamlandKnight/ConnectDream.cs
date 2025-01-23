using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/ConnectDream"), fileName = ("ConnectDream"))]
public class ConnectDream : Skill
{
	public ConnectDream()
	{
		SpCost = 15;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.connectDream(SpCost));
	}
}
