using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DreamlandKnightHolder : JobSkillHolder
{
    public static DreamlandKnightHolder Instance;
    public int DreamCount;
    public int DreamDFallenCount = 1;
    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #region  基础
    public IEnumerator dreamFallen(int SpCost)
    {
        BattleSetting.Instance.canChangeAction = false;
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.State = BattleState.Middle;
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
        DreamCount += DreamDFallenCount;
        DreamDFallenCount++;
        StartCoroutine(BattleSetting.Instance.ShowActionText("坠梦"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator oneiromancy(int SpCost)
    {
        BattleSetting.Instance.canChangeAction = false;
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.State = BattleState.Middle;
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
        StartCoroutine(BattleSetting.Instance.ShowActionText("解梦"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }
    #endregion
}
