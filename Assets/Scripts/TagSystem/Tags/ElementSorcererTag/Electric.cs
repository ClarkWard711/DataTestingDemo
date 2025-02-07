using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Electric : Tag
{
	public GameObject unit;
	public Electric()
	{
		TagName = "ElectricTag";
		TagKind = Kind.turnLessen;
		TurnAdd = 1;
		TurnLast = 1;
		BuffTarget = target.enemy;
		Effect = effect.bad;
	}

	public override void OnTurnEndCallback()
	{
		int tagPosID = unit.GetComponent<GivingData>().positionID;
		List<int> targetID = BattleSetting.Instance.CheckSurroundPosition(tagPosID);
		if (unit.CompareTag("PlayerUnit"))
		{
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				if (targetID.Exists(id => id == player.GetComponent<GivingData>().positionID) || player.GetComponent<GivingData>().positionID == tagPosID)
				{
					var damage = Mathf.CeilToInt(0.02f * player.GetComponent<GivingData>().maxHP);
					BattleSetting.Instance.DealDamageWithNoCallBack(damage, unit, player, AttackType.Soul, false);
				}
			}
		}
		else
		{
			foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				if (targetID.Exists(id => id == enemy.GetComponent<GivingData>().positionID) || enemy.GetComponent<GivingData>().positionID == tagPosID)
				{
					var damage = Mathf.CeilToInt(0.02f * enemy.GetComponent<GivingData>().maxHP);
					BattleSetting.Instance.DealDamageWithNoCallBack(damage, unit, enemy, AttackType.Soul, false);
				}
			}
		}

	}
}
