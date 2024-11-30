using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodFilthTag : Tag
{
	public float percentage = 0f;
	public GameObject unit;
	public int HitCount = 0, BeingHitCount = 0;
	public BloodFilthTag()
	{
		TagName = "BloodFilthTag";
		TagKind = Kind.accumulable;
		BuffTarget = target.self;
		Effect = effect.neutral;
		quantity = 1;
		BeforeBeingHit += SetToOne;
		BeforeHit += SetToOne;
	}

	public void SetToOne()
	{
		if (BattleSetting.Instance.CurrentActUnit == unit)
		{
			if (HitCount != 0)
			{
				if (percentage != 0)
				{
					BattleSetting.Instance.damageCache = Mathf.CeilToInt(BattleSetting.Instance.damageCache * percentage);
				}
				else
				{
					BattleSetting.Instance.damageCache = 1;
				}
				HitCount--;
				if (HitCount == 0 && BeingHitCount == 0)
				{
					quantity = 0;
				}
				return;
			}
			return;
		}
		if (BeingHitCount != 0)
		{
			BattleSetting.Instance.damageCache = 1;
			BeingHitCount--;
			if (HitCount == 0 && BeingHitCount == 0)
			{
				quantity = 0;
			}
		}
	}
}
