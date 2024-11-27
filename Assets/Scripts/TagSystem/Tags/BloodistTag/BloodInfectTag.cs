using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloodInfectTag : Tag
{
	public int tagPosID;
	public GameObject unit;
	public BloodInfectTag()
	{
		TagName = "BloodInfectTag";
		TagKind = Kind.special;
		BuffTarget = target.self;
		Effect = effect.bad;
	}

	public override void OnTurnStartCallback()
	{
		if (BloodistHolder.Instance.BloodAddictSelf >= 2)
		{
			BloodistHolder.Instance.BloodAddictSelf -= 2;
			List<int> targetID = BattleSetting.Instance.CheckSurroundPosition(tagPosID);
			tagPosID = targetID[Random.Range(0, targetID.Count)];
			targetID = BattleSetting.Instance.CheckSurroundPosition(tagPosID);
			foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				if (targetID.Exists(id => id == enemy.GetComponent<GivingData>().positionID) || enemy.GetComponent<GivingData>().positionID == tagPosID)
				{
					var damage = Mathf.CeilToInt(0.3f * BattleSetting.Instance.DamageCountingByUnit(unit, enemy, AttackType.Physical));
					BattleSetting.Instance.DealDamageWithNoCallBack(damage, unit, enemy, AttackType.Physical, false);
				}
			}
		}
		else
		{
			TagKind = Kind.accumulable;
			quantity = 0;
		}
	}
}
