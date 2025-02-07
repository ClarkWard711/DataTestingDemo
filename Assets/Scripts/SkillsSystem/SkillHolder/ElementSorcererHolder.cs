using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSorcererHolder : JobSkillHolder
{
	public ElementSorcererHolder Instance;
	public List<int> ELementCountList = new List<int>();
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
			ELementCountList.Add(0);
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
				ELementCountList[randomID]++;
				break;
			case 1:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入雷元素"));
				ELementCountList[randomID]++;
				break;
			case 2:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入水元素"));
				ELementCountList[randomID]++;
				break;
			case 3:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入冰元素"));
				ELementCountList[randomID]++;
				break;
			case 4:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入毒元素"));
				ELementCountList[randomID]++;
				break;
			case 5:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入风元素"));
				ELementCountList[randomID]++;
				break;
			case 6:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入地元素"));
				ELementCountList[randomID]++;
				break;
			case 7:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入岩元素"));
				ELementCountList[randomID]++;
				break;
			case 8:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入日元素"));
				ELementCountList[randomID]++;
				break;
			case 9:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入月元素"));
				ELementCountList[randomID]++;
				break;
			case 10:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入光元素"));
				ELementCountList[randomID]++;
				break;
			case 11:
				StartCoroutine(BattleSetting.Instance.ShowActionText("加入暗元素"));
				ELementCountList[randomID]++;
				break;
		}

		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator sorceryExplosion(int SpCost)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		float ExplosionMultiplier = 1f;
		if (ELementCountList[0] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[0]);
			//这里以及以后的buff添加要注意写一个检测是否为或者同排的判定
			if (isColumn)
			{

			}
			else if (isAOE)
			{

			}
			else
			{
				Burn tag = Burn.CreateInstance<Burn>();
				tag.TurnAdd = ELementCountList[0];
				tag.TurnLast = ELementCountList[0];
				//加入buff
			}
		}
		if (ELementCountList[1] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[1]);
			if (isColumn)
			{

			}
			else if (isAOE)
			{

			}
			else
			{
				Electric tag = Electric.CreateInstance<Electric>();
				tag.TurnAdd = ELementCountList[1];
				tag.TurnLast = ELementCountList[1];
				//加入buff
			}
		}
		if (ELementCountList[2] != 0)
		{
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				int deltaTemp;
				deltaTemp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.05f * ELementCountList[2]);
				BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().CoroutineStart(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().FloatingHP(deltaTemp));
			}
		}
		if (ELementCountList[3] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[3]);
			foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				var debuff = SpeedDown.CreateInstance<SpeedDown>();
				debuff.spd = -Mathf.CeilToInt(enemy.GetComponent<GivingData>().EnemyData.EnemyStatsList[enemy.GetComponent<GivingData>().EnemyData.EnemyLevel - 1].speed * 0.1f);
				debuff.TurnLast = 1 + ELementCountList[3];
				debuff.TurnAdd = 1 + ELementCountList[3];
				enemy.GetComponent<GivingData>().AddTagToCharacter(debuff);
			}
		}
		if (ELementCountList[4] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[4]);
		}
		if (ELementCountList[5] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[5]);
		}
		if (ELementCountList[6] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[6]);
		}
		if (ELementCountList[7] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[7]);
		}
		if (ELementCountList[8] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[8]);
		}
		if (ELementCountList[9] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[9]);
		}
		if (ELementCountList[10] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[10]);
		}
		if (ELementCountList[11] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[11]);
		}
		yield return new WaitForSeconds(1f);
		//造成伤害的方法
		BattleSetting.Instance.ActionEnd();
	}
	#endregion
}
