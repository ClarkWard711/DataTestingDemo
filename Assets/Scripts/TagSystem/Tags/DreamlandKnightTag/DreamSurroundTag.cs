using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DreamSurroundTag : Tag
{
	public GameObject unit;
	public DreamSurroundTag()
	{
		TagName = "DreamSurroundCheck";
		TagKind = Kind.turnLessen;
		Effect = effect.good;
		TurnAdd = 2;
		TurnLast = 2;
		BuffTarget = target.self;
		BeforeHit += DreamCheck;
	}

	public void DreamCheck()
	{
		int temp = unit.GetComponent<DreamlandKnightHolder>().DreamCount >= 2 ? 2 : unit.GetComponent<DreamlandKnightHolder>().DreamCount;
		unit.GetComponent<DreamlandKnightHolder>().DreamCount -= temp;
		BattleSetting.Instance.damageCache = Mathf.CeilToInt(BattleSetting.Instance.damageCache * (1 - 0.1f * temp));
	}
}
