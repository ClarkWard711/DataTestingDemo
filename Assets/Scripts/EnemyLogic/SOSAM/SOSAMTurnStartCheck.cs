using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSAMTurnStartCheck : Tag
{
	public GameObject unit;
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

				}
			}
		}
	}
	public void DamageDecrease()
	{

	}
}
