using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/MoonDuke"), fileName = ("MoonDuke"))]

public class MoonDuke : OdorikoSkill
{
    public MoonDuke()
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
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.moonDuke(SpCost, odoSkillKind));
    }
}
