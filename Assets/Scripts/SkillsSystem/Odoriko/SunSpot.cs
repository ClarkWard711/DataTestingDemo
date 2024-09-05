using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunSpot"), fileName = ("SunSpot"))]
public class SunSpot : OdorikoSkill
{
    public SunSpot()
    {
        SpCost = 5;
        odoSkillKind = OdoSkillKind.Sun;
        DealMultiplier = 0.9f;
    }
    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunSpot(SpCost, odoSkillKind));
    }
    /*IEnumerator sunSpot()
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().BuffList.Exists(Buff => Buff.BuffName == "蓄力"))
        {
            SunSpReduce = true;
            SpCostMultiplier = 0.8f;
        }
        else
        {
            SunSpReduce = true;
            SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.GameStateText.text = "日斑";
        StartCoroutine(BattleSetting.Instance.ShowText(1f));
        StartCoroutine(BattleSetting.Instance.DealDamage(3f));
    }*/
}
