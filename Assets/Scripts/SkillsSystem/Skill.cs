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
        if (BattleSetting.Instance.State == BattleState.Middle)
        {
            //Debug.Log(000);
            return;
        }
        BattleSetting.Instance.canChangeAction = true;
    }
}
