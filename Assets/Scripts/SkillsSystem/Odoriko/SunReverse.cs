using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunReverse"), fileName = ("SunReverse"))]

public class SunReverse : OdorikoSkill
{
    public SunReverse()
    {
        SpCost = 18;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunReverse(SpCost, odoSkillKind));
    }
}
