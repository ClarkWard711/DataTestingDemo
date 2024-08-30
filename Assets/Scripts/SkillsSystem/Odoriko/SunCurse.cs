using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCurse : OdorikoSkill
{
    public SunCurse()
    {
        SpCost = 18;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunCurse(SpCost, odoSkillKind));
    }
}
