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
    public Text[] BasicSkillText;
    public Text[] AdvancedSkillText;
    void Awake()
    {
        BasicSkillButton = BattleSetting.Instance.BasicPanel.GetComponentsInChildren<Button>();
        AdvancedSkillButton = BattleSetting.Instance.AdvancedPanel.GetComponentsInChildren<Button>();
    }

    public void AddSkillToButton()
    {
        BasicSkillButton[0].onClick.AddListener(() => JobSkill.skillList[0].Apply(BattleSetting.Instance.CurrentActUnit));
        BasicSkillButton[1].onClick.AddListener(() => JobSkill.skillList[1].Apply(BattleSetting.Instance.CurrentActUnit));

        for (int i = 0; i < 4; i++)
        {
            AdvancedSkillButton[0].interactable = true;

            if (jobData.SkillsID[i] != -1) 
            {
                //把按钮文字也给改了
                AdvancedSkillButton[0].onClick.AddListener(() => JobSkill.skillList[jobData.SkillsID[i]].Apply(BattleSetting.Instance.CurrentActUnit));

                if (JobSkill.skillList[jobData.SkillsID[i]].SpCost<BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
                {
                    AdvancedSkillButton[0].interactable = false;
                }
            }
        }
    }

    /*public virtual void SpCounter(int SpCost)
    {
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
    }*/
    public virtual void ActionEndCallback()
    {
        StartCoroutine(ActionEnd());
        BattleSetting.Instance.ToBattle();
    }

    public void CoroutineStart(IEnumerator enumerator)
    {
        StartCoroutine(enumerator);
    }

    IEnumerator ActionEnd()
    {
        foreach (var tag in BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList)
        {
            var method = tag.GetType().GetMethod("OnActionEndCallback");
            if (method.DeclaringType == typeof(Tag))
            {
                // Tag does not override OnTurnEndCallback, so skip to the next tag
                continue;
            }
            tag.OnTurnEndCallback();

            yield return StartCoroutine(BattleSetting.Instance.DelayedCallback(2f));
        }
        BattleSetting.Instance.isActionEnding = false;
    }
}
