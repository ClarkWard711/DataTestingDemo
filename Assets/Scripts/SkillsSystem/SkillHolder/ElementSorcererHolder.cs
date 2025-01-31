using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSorcererHolder : JobSkillHolder
{
	public ElementSorcererHolder Instance;
	public List<int> ELementCountList = new List<int>();
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
			Burn tag = Burn.CreateInstance<Burn>();
			tag.TurnAdd = ELementCountList[0];
			tag.TurnLast = ELementCountList[0];
		}
		if (ELementCountList[1] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[1]);
		}
		if (ELementCountList[2] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[2]);
		}
		if (ELementCountList[3] != 0)
		{
			ExplosionMultiplier += (0.5f * ELementCountList[3]);
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
		BattleSetting.Instance.ActionEnd();
	}
	#endregion
}
