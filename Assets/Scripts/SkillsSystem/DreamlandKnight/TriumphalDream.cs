using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/DreamlanKnightSkill/TriumphalDream"), fileName = ("TriumphalDream"))]
public class TriumphalDream : Skill
{
	public TriumphalDream()
	{
		SpCost = 18;
	}

	public override void Apply(GameObject unit)
	{
		base.Apply(unit);
		BattleSetting.Instance.State = BattleState.Middle;
		DreamlandKnightHolder.Instance.CoroutineStart(DreamlandKnightHolder.Instance.triumphalDream(SpCost));
	}
}
