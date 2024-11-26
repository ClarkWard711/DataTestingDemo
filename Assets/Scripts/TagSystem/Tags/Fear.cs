using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fear : Tag
{
	public GameObject unit;
	public Fear()
	{
		TagName = "Fear";
		Impact = impactOnMultiplier.AllDeal;
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.enemy;
		Effect = effect.bad;
		TurnLast = 1;
		OnTagAdded += ChangePosition;
	}

	public void ChangePosition()
	{
		if (unit.GetComponent<GivingData>().tagList.Exists(tag => tag is Melee))
		{
			if (unit.CompareTag("PlayerUnit"))
			{
				int i = unit.GetComponent<GivingData>().positionID + 3;
				if (BattleSetting.Instance.PlayerPositionsList[i].transform.childCount != 0)
				{
					BattleSetting.Instance.PlayerPositionsList[i].transform.GetChild(0).gameObject.transform.SetParent(unit.transform.parent, false);
					unit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[i].transform, false);
				}
				else
				{
					unit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[i].transform, false);
				}
				BattleSetting.Instance.CheckPositionID();
				BattleSetting.Instance.ComparePosition();
			}
			else
			{
				int i = unit.GetComponent<GivingData>().positionID + 3;
				if (BattleSetting.Instance.EnemyPositionsList[i].transform.childCount != 0)
				{
					BattleSetting.Instance.EnemyPositionsList[i].transform.GetChild(0).gameObject.transform.SetParent(unit.transform.parent, false);
					unit.transform.SetParent(BattleSetting.Instance.EnemyPositionsList[i].transform, false);
				}
				else
				{
					unit.transform.SetParent(BattleSetting.Instance.EnemyPositionsList[i].transform, false);
				}
				BattleSetting.Instance.CheckPositionID();
				BattleSetting.Instance.ComparePosition();
			}
		}
	}
}
