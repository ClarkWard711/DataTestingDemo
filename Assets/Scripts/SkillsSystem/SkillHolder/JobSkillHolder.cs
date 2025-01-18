using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class JobSkillHolder : MonoBehaviour
{
	public SkillContainer JobSkill;
	public JobData jobData;
	public Button[] BasicSkillButton;
	public Button[] AdvancedSkillButton;
	public GameObject SpecialSkill;
	public Button SpecialButton;
	public Text[] BasicSkillText;
	public Text[] AdvancedSkillText;
	public List<int> coolDownList;
	public virtual void Awake()
	{
		BasicSkillButton = BattleSetting.Instance.BasicPanel.GetComponentsInChildren<Button>();
		AdvancedSkillButton = BattleSetting.Instance.AdvancedPanel.GetComponentsInChildren<Button>();
		SpecialSkill = BattleSetting.Instance.SpecialSkill;
		SpecialButton = SpecialSkill.GetComponent<Button>();
		jobData = GetComponent<GivingData>().jobData;
		//如果有技能冷却 则在jobData中找对应id在哪一位之后对应的cooldownlist位置增加冷却 用list.indexof()的方法检索
		coolDownList.Clear();
		for (int i = 0; i < 4; i++)
		{
			coolDownList.Add(0);
		}
	}

	public virtual void AddSkillToButton()
	{
		if (JobSkill != null)
		{
			BasicSkillButton[0].onClick.RemoveAllListeners();
			BasicSkillButton[1].onClick.RemoveAllListeners();
			SpecialButton.onClick.RemoveAllListeners();
			BasicSkillButton[0].onClick.AddListener(() => JobSkill.skillList[0].Apply(BattleSetting.Instance.CurrentActUnit));
			BasicSkillButton[1].onClick.AddListener(() => JobSkill.skillList[1].Apply(BattleSetting.Instance.CurrentActUnit));
			BattleSetting.Instance.BasicPanel.GetComponentsInChildren<FloatingText>()[0].description = JobSkill.skillList[0].Description;
			BattleSetting.Instance.BasicPanel.GetComponentsInChildren<FloatingText>()[1].description = JobSkill.skillList[1].Description;
			BasicSkillButton[0].GetComponentInChildren<Text>().text = JobSkill.skillList[0].SkillName;
			BasicSkillButton[1].GetComponentInChildren<Text>().text = JobSkill.skillList[1].SkillName;
			if (JobSkill.skillList[0].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				BasicSkillButton[0].interactable = false;
			}
			if (JobSkill.skillList[1].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				BasicSkillButton[1].interactable = false;
			}
			for (int i = 0; i < 4; i++)
			{
				AdvancedSkillButton[i].interactable = true;
				AdvancedSkillButton[i].onClick.RemoveAllListeners();
				if (jobData.SkillsID[i] != -1)
				{
					//把按钮文字也给改了
					AdvancedSkillButton[i].GetComponentInChildren<Text>().text = JobSkill.skillList[jobData.SkillsID[i]].SkillName;
					BattleSetting.Instance.AdvancedPanel.GetComponentsInChildren<FloatingText>()[i].description = JobSkill.skillList[jobData.SkillsID[i]].Description;
					//AdvancedSkillButton[i].onClick.AddListener(() => JobSkill.skillList[jobData.SkillsID[i]].Apply(BattleSetting.Instance.CurrentActUnit));
					if (JobSkill.skillList[jobData.SkillsID[i]].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
					{
						AdvancedSkillButton[i].interactable = false;
					}
					//技能冷却中
					if (coolDownList[i] > 0)
					{
						AdvancedSkillButton[i].interactable = false;
					}
				}
			}
			AdvancedSkillButton[0].onClick.AddListener(() => JobSkill.skillList[jobData.SkillsID[0]].Apply(BattleSetting.Instance.CurrentActUnit));
			AdvancedSkillButton[1].onClick.AddListener(() => JobSkill.skillList[jobData.SkillsID[1]].Apply(BattleSetting.Instance.CurrentActUnit));
			AdvancedSkillButton[2].onClick.AddListener(() => JobSkill.skillList[jobData.SkillsID[2]].Apply(BattleSetting.Instance.CurrentActUnit));
			AdvancedSkillButton[3].onClick.AddListener(() => JobSkill.skillList[jobData.SkillsID[3]].Apply(BattleSetting.Instance.CurrentActUnit));

			if (jobData.SpecialID != 0)
			{
				SpecialSkill.GetComponentInChildren<Text>().text = JobSkill.skillList[jobData.SpecialID].SkillName;
				SpecialButton.onClick.AddListener(() => JobSkill.skillList[jobData.SpecialID].Apply(BattleSetting.Instance.CurrentActUnit));
				SpecialButton.GetComponent<FloatingText>().enabled = true;
				SpecialSkill.GetComponentInChildren<FloatingText>().description = JobSkill.skillList[jobData.SpecialID].Description;
			}
			else
			{
				SpecialSkill.GetComponentInChildren<Text>().text = "无";
				SpecialSkill.GetComponentInChildren<FloatingText>().enabled = false;
				SpecialSkill.GetComponent<Button>().interactable = false;
			}
		}
		if (BattleSetting.Instance.isTextShowed)
		{
			BattleSetting.Instance.OnSKillButton();
		}
		//BattleSetting.Instance.OnSKillButton();
	}

	/*public virtual void SpCounter(int SpCost)
	{
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
	}*/
	public virtual void AttackButton()
	{
		StartCoroutine(Attack());
	}

	public virtual void ActionEndCallback()
	{
		StartCoroutine(ActionEnd());
	}

	public void CoroutineStart(IEnumerator enumerator)
	{
		StartCoroutine(enumerator);
	}

	IEnumerator Attack()
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.isNormalAttack = true;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Physical;
		StartCoroutine(BattleSetting.Instance.DealDamage(3f, false));
	}

	IEnumerator ActionEnd()
	{
		for (int i = 0; i < coolDownList.Count; i++)
		{
			if (coolDownList[i] > 0)
			{
				coolDownList[i]--;
			}
		}
		foreach (var tag in BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList)
		{
			var method = tag.GetType().GetMethod("OnActionEndCallback");
			if (method.DeclaringType == typeof(Tag))
			{
				// Tag does not override OnTurnEndCallback, so skip to the next tag
				continue;
			}
			tag.OnActionEndCallback();

			yield return StartCoroutine(BattleSetting.Instance.DelayedCallback(1.5f));
		}
		BattleSetting.Instance.isActionEnding = false;
		BattleSetting.Instance.ToBattle();
		//Debug.Log(this.gameObject.name);
		foreach (var button in AdvancedSkillButton)
		{
			button.GetComponent<FloatingText>().DestroyPanel();
		}
		foreach (var button in BasicSkillButton)
		{
			button.GetComponent<FloatingText>().DestroyPanel();
		}
	}
}
