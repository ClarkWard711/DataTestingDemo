using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonErode : OdorikoSkill
{
    public MoonErode()
    {
        odoSkillKind = OdoSkillKind.Moon;
        SpCost = 12;
    }

    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.moonErode(SpCost, odoSkillKind));
    }
}
