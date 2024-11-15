using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bleed : Tag
{
	public bool isSelf;
	public int hpOfUnit;
	public GameObject unit;
	public Bleed()
	{
		TagName = "Bleed";
		Impact = impactOnMultiplier.AllDeal;
		TurnAdd = 1;
		TagKind = Kind.turnLessen;
		BuffTarget = target.enemy;
		Effect = effect.bad;
		TurnLast = 1;
		OnDealDamage += BleedActivate;
	}

	public void BleedActivate()
	{
		BattleSetting.Instance.DealDamageWithNoCallBack(Mathf.CeilToInt(hpOfUnit * 0.04f), null, unit, AttackType.Physical, isSelf);
		//这里的gameobject可能需要传入 先看一下目前的行不行
	}
}
