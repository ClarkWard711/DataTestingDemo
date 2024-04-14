using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/Moonlight"), fileName = ("SunSpot"))]
public class SunSpot : OdorikoSkill
{
    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;

        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        OdorikoHolder.Instance.sunSpotCoroutine(SpCost,odoSkillKind);
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
