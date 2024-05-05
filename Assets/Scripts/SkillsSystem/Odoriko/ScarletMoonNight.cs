using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/ScarletMoonNight"), fileName = ("ScarletMoonNight"))]

public class ScarletMoonNight : OdorikoSkill
{
    public ScarletMoonNight()
    {
        odoSkillKind = OdoSkillKind.Moon;
        SpCost = 8;
    }

    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.scarletMoon(SpCost,odoSkillKind));
    }
}
