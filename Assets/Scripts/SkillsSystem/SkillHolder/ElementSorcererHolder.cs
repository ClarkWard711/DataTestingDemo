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
	#endregion
}
