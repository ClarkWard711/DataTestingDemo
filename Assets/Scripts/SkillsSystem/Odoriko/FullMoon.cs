using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMoon : OdorikoSkill
{
    public FullMoon()
    {
        SpCost = 15;
        odoSkillKind = OdoSkillKind.Moon;
    }

    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.fullMoon(SpCost, odoSkillKind));
    }
}
