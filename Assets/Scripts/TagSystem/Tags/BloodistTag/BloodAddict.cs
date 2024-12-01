using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OdorikoTag;

public class BloodAddict : Tag
{
	public bool isNoSelf = false;
	public bool isBloodist = false;
	public BloodAddict()
	{
		TagName = "BloodAddict";
		TagKind = Kind.special;
		Effect = effect.neutral;
		BuffTarget = target.all;
		OnSelfDamageTake += BloodAddictCheck;
		OnDamageTake += BloodAddictEnemyCheck;
	}

	public void BloodAddictCheck()
	{
		BloodistHolder.Instance.BloodAddictSelf++;
	}

	public void BloodAddictEnemyCheck()
	{
		if (isNoSelf || isBloodist)
		{
			BloodistHolder.Instance.BloodAddictEnemy++;
		}
	}
}
