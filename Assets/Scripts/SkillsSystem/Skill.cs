using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillKind {Baic,Advanced,Special};
public class Skill : ScriptableObject
{
    public int SkillID;
    public int SpCost;
    public int TurnCooldown;
    public float DealMultiplier;
    public string SkillName;
    public string Description;
    public float HitRevise;
    public virtual void Apply(GameObject unit)
    {
        BattleSetting.Instance.BasicPanel.SetActive(false);
        BattleSetting.Instance.isBasicShowed = false;
        BattleSetting.Instance.AdvancedPanel.SetActive(false);
        BattleSetting.Instance.isAdvancedShowed = false;
        //BattleSetting.Instance.StopAllCoroutines();
        BattleSetting.Instance.CheckCanChangeAction();
        foreach (var button in BattleSetting.Instance.CurrentActUnit.GetComponent<JobSkillHolder>().AdvancedSkillButton)
        {
            button.GetComponent<FloatingText>().DestroyPanel();
        }
        foreach (var button in BattleSetting.Instance.CurrentActUnit.GetComponent<JobSkillHolder>().BasicSkillButton)
        {
            button.GetComponent<FloatingText>().DestroyPanel();
        }
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        BattleSetting.Instance.canChangeAction = true;
    }
}
