using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class SOSAMTurnStartCheck : Tag
{
	public GameObject unit;
	public float CounterMultiplier = 0.1f;
	public SOSAMTurnStartCheck()
	{
		TagName = "SOSAMCheck";
		TagKind = Kind.eternal;
		BuffTarget = target.self;
		Effect = effect.neutral;
		BeforeBeingHit += DamageDecrease;
	}

	public override void OnTurnStartCallback()
	{
		if (unit.GetComponent<ShadowOfSunAndMoon>().BossState == ShadowOfSunAndMoon.SOSAMState.Tsuki)
		{
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				if (player.GetComponent<GivingData>().tagList.Exists(tag => tag is Remote))
				{
					int num = Mathf.CeilToInt(player.GetComponent<GivingData>().maxSP * 0.02f);
					unit.GetComponent<GivingData>().CoroutineStart(player.GetComponent<GivingData>().FloatingSP(-num));
					SpeedDown tag = SpeedDown.CreateInstance<SpeedDown>();
					tag.spd = -Mathf.CeilToInt(Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f));
					tag.TurnLast = 2;
					player.GetComponent<GivingData>().AddTagToCharacter(tag);
				}
			}
		}
		else
		{
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				if (player.GetComponent<GivingData>().tagList.Exists(tag => tag is Melee))
				{
					Burn tag = Burn.CreateInstance<Burn>();
					tag.unit = player;
					player.GetComponent<GivingData>().AddTagToCharacter(tag);
					SOSAMSunTag tag1 = SOSAMSunTag.CreateInstance<SOSAMSunTag>();
					SOSAMSunTagSecond tag2 = SOSAMSunTagSecond.CreateInstance<SOSAMSunTagSecond>();
					player.GetComponent<GivingData>().AddTagToCharacter(tag1);
					player.GetComponent<GivingData>().AddTagToCharacter(tag2);
				}
			}
		}
	}
	public void DamageDecrease()
	{
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType == AttackType.Soul && unit.GetComponent<ShadowOfSunAndMoon>().BossState == ShadowOfSunAndMoon.SOSAMState.Tsuki)
		{
			BattleSetting.Instance.damageCache = 0;
			//眩晕也为0
		}
		else if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType == AttackType.Physical && unit.GetComponent<ShadowOfSunAndMoon>().BossState == ShadowOfSunAndMoon.SOSAMState.Hi)
		{
			BattleSetting.Instance.damageCache = 0;
			//眩晕也为0
		}
		if (BattleSetting.Instance.damageCache != 0)
		{
			if (unit.GetComponent<ShadowOfSunAndMoon>().BossState == ShadowOfSunAndMoon.SOSAMState.Hi)
			{
				int damage = Mathf.CeilToInt(BattleSetting.Instance.DamageCountingByUnit(unit, BattleSetting.Instance.CurrentActUnit, AttackType.Physical) * CounterMultiplier);
				BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.DealCounterDamage(damage, AttackType.Physical));
			}
		}
	}
}
