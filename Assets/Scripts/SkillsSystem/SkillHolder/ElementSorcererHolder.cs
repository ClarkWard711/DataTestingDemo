using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSorcererHolder : JobSkillHolder
{
	public ElementSorcererHolder Instance;
	public List<int> ElementCountList = new List<int>();
	public bool isAOE;
	public bool isColumn;
	public override void Awake()
	{
		base.Awake();
		if (Instance == null)
		{
			Instance = this;
		}
		for (int i = 0; i < 12; i++)
		{
			ElementCountList.Add(0);
		}
	}

	#region 基础
	public IEnumerator sorceryBoost(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		int randomID = Random.Range(0, 12);

		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		switch (randomID)
		{
			case 0:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入火元素"));
				ElementCountList[randomID]++;
				break;
			case 1:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入雷元素"));
				ElementCountList[randomID]++;
				break;
			case 2:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入水元素"));
				ElementCountList[randomID]++;
				break;
			case 3:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入冰元素"));
				ElementCountList[randomID]++;
				break;
			case 4:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入毒元素"));
				ElementCountList[randomID]++;
				break;
			case 5:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入风元素"));
				ElementCountList[randomID]++;
				break;
			case 6:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入地元素"));
				ElementCountList[randomID]++;
				break;
			case 7:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入岩元素"));
				ElementCountList[randomID]++;
				break;
			case 8:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入日元素"));
				ElementCountList[randomID]++;
				break;
			case 9:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入月元素"));
				ElementCountList[randomID]++;
				break;
			case 10:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入光元素"));
				ElementCountList[randomID]++;
				break;
			case 11:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入暗元素"));
				ElementCountList[randomID]++;
				break;
		}

		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator sorceryExplosion(int SpCost)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		float ExplosionMultiplier = 1f;
		if (ElementCountList[0] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[0]);
			//这里以及以后的buff添加要注意写一个检测是否为或者同排的判定
			if (isColumn && !isAOE)
			{
				foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
				{
					if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
					{
						if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag is Melee))
						{
							var debuff = Burn.CreateInstance<Burn>();
							debuff.TurnAdd = ElementCountList[0];
							debuff.TurnLast = ElementCountList[0];
							enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
						}
					}
					else if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag is Remote))
					{
						if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
						{
							var debuff = Burn.CreateInstance<Burn>();
							debuff.TurnAdd = ElementCountList[0];
							debuff.TurnLast = ElementCountList[0];
							enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
						}
					}
				}
			}
			else if (isAOE)
			{
				foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
				{
					var debuff = Burn.CreateInstance<Burn>();
					debuff.TurnAdd = ElementCountList[0];
					debuff.TurnLast = ElementCountList[0];
					enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
				}
			}
			else
			{
				Burn tag = Burn.CreateInstance<Burn>();
				tag.TurnAdd = ElementCountList[0];
				tag.TurnLast = ElementCountList[0];
				//加入buff
				BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
		}
		if (ElementCountList[1] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[1]);
			if (isColumn && !isAOE)
			{
				foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
				{
					if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
					{
						if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag is Melee))
						{
							var debuff = Electric.CreateInstance<Electric>();
							debuff.TurnAdd = ElementCountList[1];
							debuff.TurnLast = ElementCountList[1];
							enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
						}
					}
					else if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag is Remote))
					{
						if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
						{
							var debuff = Electric.CreateInstance<Electric>();
							debuff.TurnAdd = ElementCountList[1];
							debuff.TurnLast = ElementCountList[1];
							enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
						}
					}
				}
			}
			else if (isAOE)
			{
				foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
				{
					var debuff = Electric.CreateInstance<Electric>();
					debuff.TurnAdd = ElementCountList[1];
					debuff.TurnLast = ElementCountList[1];
					enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
				}
			}
			else
			{
				Electric tag = Electric.CreateInstance<Electric>();
				tag.TurnAdd = ElementCountList[1];
				tag.TurnLast = ElementCountList[1];
				//加入buff
				BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
		}
		if (ElementCountList[2] != 0)
		{
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				int deltaTemp;
				deltaTemp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.08f * ElementCountList[2]);
				BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().CoroutineStart(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().FloatingHP(deltaTemp));
			}
		}
		if (ElementCountList[3] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[3]);
			foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				var debuff = SpeedDown.CreateInstance<SpeedDown>();
				debuff.spd = -Mathf.CeilToInt(enemy.GetComponent<GivingData>().EnemyData.EnemyStatsList[enemy.GetComponent<GivingData>().EnemyData.EnemyLevel - 1].speed * 0.1f);
				debuff.TurnLast = 1 + ElementCountList[3];
				debuff.TurnAdd = 1 + ElementCountList[3];
				enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
			}
		}
		if (ElementCountList[4] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[4]);
			if (isColumn && !isAOE)
			{
				foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
				{
					if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
					{
						if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag is Melee))
						{
							for (int i = 0; i < ElementCountList[4]; i++)
							{
								Poison debuff = Poison.CreateInstance<Poison>();
								debuff.unit = enemy;
								debuff.percentage = 0.04f;
								debuff.TurnAdd = 2;
								debuff.TurnLast = 2;
								enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
							}
						}
					}
					else if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag is Remote))
					{
						if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
						{
							for (int i = 0; i < ElementCountList[4]; i++)
							{
								Poison debuff = Poison.CreateInstance<Poison>();
								debuff.unit = enemy;
								debuff.percentage = 0.04f;
								debuff.TurnAdd = 2;
								debuff.TurnLast = 2;
								enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
							}
						}
					}
				}
			}
			else if (isAOE)
			{
				foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
				{
					for (int i = 0; i < ElementCountList[4]; i++)
					{
						Poison debuff = Poison.CreateInstance<Poison>();
						debuff.unit = enemy;
						debuff.percentage = 0.04f;
						debuff.TurnAdd = 2;
						debuff.TurnLast = 2;
						enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
					}
				}
			}
			else
			{
				Poison tag = Poison.CreateInstance<Poison>();
				tag.unit = BattleSetting.Instance.CurrentActUnitTarget;
				tag.percentage = 0.04f;
				tag.TurnAdd = 2;
				tag.TurnLast = 2;
				//加入buff
				BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
		}
		if (ElementCountList[5] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[5]);
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				var buff = SpeedUP.CreateInstance<SpeedUP>();
				buff.spd = Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f);
				buff.TurnLast = 1 + ElementCountList[5];
				buff.TurnAdd = 1 + ElementCountList[5];
				player.GetComponent<GivingData>().AddTagToCharacter(buff);
			}
		}
		if (ElementCountList[6] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[6]);
		}
		if (ElementCountList[7] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[7]);
		}
		if (ElementCountList[8] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[8]);
		}
		if (ElementCountList[9] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[9]);
		}
		if (ElementCountList[10] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[10]);
		}
		if (ElementCountList[11] != 0)
		{
			ExplosionMultiplier += (0.5f * ElementCountList[11]);
		}
		yield return new WaitForSeconds(0.2f);
		if (isColumn && !isAOE)
		{
			foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
				{
					if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag is Melee))
					{
						int baseDamage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Soul);
						int bonusDamage = Mathf.CeilToInt(baseDamage * ExplosionMultiplier);
						BattleSetting.Instance.DealDamageExtra(bonusDamage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Soul, false);
					}
				}
				else if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag is Remote))
				{
					if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
					{
						int baseDamage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Soul);
						int bonusDamage = Mathf.CeilToInt(baseDamage * ExplosionMultiplier);
						BattleSetting.Instance.DealDamageExtra(bonusDamage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Soul, false);
					}
				}
			}
		}
		else if (isAOE)
		{
			foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				int baseDamage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Soul);
				int bonusDamage = Mathf.CeilToInt(baseDamage * ExplosionMultiplier);
				BattleSetting.Instance.DealDamageBonus(bonusDamage, AttackType.Soul);
			}
		}
		else
		{
			int baseDamage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Soul);
			int bonusDamage = Mathf.CeilToInt(baseDamage * ExplosionMultiplier);
			BattleSetting.Instance.DealDamageBonus(bonusDamage, AttackType.Soul);
		}
		yield return new WaitForSeconds(0.3f);
		if (ElementCountList[7] != 0)
		{
			for (int i = 0; i < ElementCountList[7]; i++)
			{
				int baseDamage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Soul);
				int bonusDamage = Mathf.CeilToInt(baseDamage * 0.5f);
				BattleSetting.Instance.DealDamageBonus(bonusDamage, AttackType.Soul);
				yield return new WaitForSeconds(0.3f);
			}
		}
		StartCoroutine(BattleSetting.Instance.OnDealDamage());
		isColumn = false;
		isAOE = false;
		for (int i = 0; i < 12; i++)
		{
			ElementCountList[i] = 0;
		}
		BattleSetting.Instance.ActionEnd();
	}
	#endregion
}
