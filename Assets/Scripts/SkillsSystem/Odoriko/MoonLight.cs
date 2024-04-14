using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/Moonlight"), fileName = ("MoonLight"))]
public class MoonLight : OdorikoSkill
{
    
    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;

        OdorikoHolder.Instance.SpCounter(SpCost,odoSkillKind);
        Buff defencing = new Buff(BattleSetting.Instance.Defencing);
        unit.GetComponent<GivingData>().BuffList.Add(defencing);
        BattleSetting.Instance.CheckBuffList(unit);
        defencing.TurnLast++;
        if (unit.GetComponent<GivingData>().BuffList.Exists(Buff => Buff.BuffName == "蓄力"))
        {
            OdorikoHolder.Instance.MoonSpReduce = true;
            OdorikoHolder.Instance.SpCostMultiplier = 0.8f;
        }
        else
        {
            OdorikoHolder.Instance.MoonSpReduce = true;
            OdorikoHolder.Instance.SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.State = BattleState.Middle;
        //StartCoroutine(BattleSetting.Instance.ShowActionText("月光"));
        
    }
}
