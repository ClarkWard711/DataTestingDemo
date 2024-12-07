using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum SkillKind { Baic, Advanced, Special };
public class Skill : ScriptableObject
{
	public int SkillID;
	public int SpCost;
	public int TurnCooldown;
	public float DealMultiplier;
	public string SkillName;
	[TextArea]
	public string Description;
	public float HitRevise;
	public virtual void Apply(GameObject unit)
	{
		BattleSetting.Instance.BasicPanel.SetActive(false);
		BattleSetting.Instance.isBasicShowed = false;
		BattleSetting.Instance.AdvancedPanel.SetActive(false);
		BattleSetting.Instance.isAdvancedShowed = false;
		foreach (var button in BattleSetting.Instance.DelimaPanel.GetComponentsInChildren<Button>())
		{
			Destroy(button.gameObject);
		}
		BattleSetting.Instance.DelimaPanel.SetActive(false);
		//BattleSetting.Instance.StopAllCoroutines();

		foreach (var button in BattleSetting.Instance.CurrentActUnit.GetComponent<JobSkillHolder>().AdvancedSkillButton)
		{
			if (button.GetComponent<FloatingText>() != null)
			{
				button.GetComponent<FloatingText>().DestroyPanel();
			}
		}
		foreach (var button in BattleSetting.Instance.CurrentActUnit.GetComponent<JobSkillHolder>().BasicSkillButton)
		{
			if (button.GetComponent<FloatingText>() != null)
			{
				button.GetComponent<FloatingText>().DestroyPanel();
			}
		}
		BattleSetting.Instance.CheckCanChangeAction();
		if (BattleSetting.Instance.State == BattleState.Middle || BattleSetting.Instance.State == BattleState.EnemyTurn)
		{
			//Debug.Log("Return");
			return;
		}
		else
		{
			BattleSetting.Instance.canChangeAction = true;
		}
	}
}
