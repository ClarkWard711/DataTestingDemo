using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowOfSunAndMoon : Enemy
{
	public enum SOSAMState { Tsuki, Hi, Final }
	public Sprite TsukiMode;
	public Sprite HiMode;
	public SOSAMState BossState = SOSAMState.Tsuki;
	public int TurnCount;

	public override void Awake()
	{
		base.Awake();
		SOSAMTurnStartCheck tag = SOSAMTurnStartCheck.CreateInstance<SOSAMTurnStartCheck>();
		tag.unit = this.gameObject;
		givingData.AddTagToCharacter(tag);
	}
	public override void EnemyAction()
	{
		StartCoroutine(BossLogic());
	}

	IEnumerator BossLogic()
	{
		if (givingData.currentHP <= givingData.maxHP * 0.8f && givingData.currentHP > givingData.maxHP * 0.6f)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = HiMode;
			BossState = SOSAMState.Hi;
		}
		else if (givingData.currentHP <= givingData.maxHP * 0.6f && givingData.currentHP > givingData.maxHP * 0.4f)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = TsukiMode;
			BossState = SOSAMState.Tsuki;
		}
		else if (givingData.currentHP <= givingData.maxHP * 0.4f && givingData.currentHP > givingData.maxHP * 0.2f)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = HiMode;
			BossState = SOSAMState.Hi;
		}

		if (givingData.currentHP > givingData.maxHP * 0.2f)
		{
			//20%之前的逻辑
			if (BossState == SOSAMState.Tsuki)
			{
				TurnCount = 0;
				((SOSAMTurnStartCheck)givingData.tagList.Find(tag => tag is SOSAMTurnStartCheck)).CounterMultiplier = 0.1f;
				if (givingData.tagList.FindAll(tag => tag.Effect == Tag.effect.bad).Count >= 3)
				{
					//去除buff并回血
					givingData.CoroutineStart(givingData.FloatingHP(2000));
					foreach (var negative in givingData.tagList.FindAll(tag => tag.Effect == Tag.effect.bad))
					{
						givingData.tagList.Remove(negative);
					}
					yield return new WaitForSeconds(1f);
					StartCoroutine(BattleSetting.Instance.ShowActionText("去除所有负面效果"));
				}
				else if (!givingData.tagList.Exists(tag => tag is SoulAtkUp))
				{
					//获得魔法伤害提升
					SoulAtkUp tag = SoulAtkUp.CreateInstance<SoulAtkUp>();
					tag.TurnAdd += 2;
					tag.TurnLast += 2;
					givingData.AddTagToCharacter(tag);
					yield return new WaitForSeconds(1f);
					StartCoroutine(BattleSetting.Instance.ShowActionText("获得魔法伤害提升"));
				}
				else
				{
					//造成伤害并移位置
					var RandomUnit = BattleSetting.Instance.RemainingPlayerUnits[Random.Range(0, BattleSetting.Instance.RemainingPlayerUnits.Length)];
					int randomID = Random.Range(0, 6);
					if (BattleSetting.Instance.PlayerPositionsList[randomID].transform.childCount != 0)
					{
						BattleSetting.Instance.PlayerPositionsList[randomID].transform.GetChild(0).gameObject.transform.SetParent(RandomUnit.transform.parent, false);
						RandomUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[randomID].transform, false);
					}
					else
					{
						RandomUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[randomID].transform, false);
					}
					BattleSetting.Instance.CheckPositionID();
					BattleSetting.Instance.ComparePosition();
					float randomPosibility = Random.Range(0, 1f);
					if (randomPosibility <= 0.4f)
					{
						foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
						{
							int damage = BattleSetting.Instance.DamageCountingByUnit(gameObject, player, AttackType.Soul);
							BattleSetting.Instance.DealDamageExtra(damage, gameObject, player, AttackType.Soul, false);
						}
						yield return new WaitForSeconds(1f);
					}
					else
					{
						foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
						{
							if (player.GetComponent<GivingData>().positionID == 0 || player.GetComponent<GivingData>().positionID == 2 || player.GetComponent<GivingData>().positionID == 3 || player.GetComponent<GivingData>().positionID == 5)
							{
								int damage = Mathf.CeilToInt(BattleSetting.Instance.DamageCountingByUnit(gameObject, player, AttackType.Soul) * 1.5f);
								BattleSetting.Instance.DealDamageExtra(damage, gameObject, player, AttackType.Soul, false);
							}
						}
						yield return new WaitForSeconds(1f);
					}
				}
			}
			else
			{
				if (TurnCount == 0)
				{
					TurnCount++;
					SoulAtkDown tag = SoulAtkDown.CreateInstance<SoulAtkDown>();
					tag.TurnAdd++;
					tag.TurnLast++;
					foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
					{
						player.GetComponent<GivingData>().AddTagToCharacter(tag);
					}
					SoulDfsUp tag0 = SoulDfsUp.CreateInstance<SoulDfsUp>();
					tag0.TurnAdd = 4;
					tag0.TurnLast = 4;
					givingData.AddTagToCharacter(tag0);
					yield return new WaitForSeconds(1f);
					StartCoroutine(BattleSetting.Instance.ShowActionText("去除所有负面效果"));
				}
				else if (TurnCount == 1)
				{
					TurnCount++;
					foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
					{
						HitDown tag0 = HitDown.CreateInstance<HitDown>();
						tag0.hit = -Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].hit * 0.05f);
						CriDown tag1 = CriDown.CreateInstance<CriDown>();
						tag1.cri = -Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].critical * 0.05f);
						player.GetComponent<GivingData>().AddTagToCharacter(tag0);
						player.GetComponent<GivingData>().AddTagToCharacter(tag1);
					}

					((SOSAMTurnStartCheck)givingData.tagList.Find(tag => tag is SOSAMTurnStartCheck)).CounterMultiplier = 0.25f;
				}
			}
		}
		else
		{
			//小于20%的逻辑
		}
		BattleSetting.Instance.ActionEnd();
	}
}
