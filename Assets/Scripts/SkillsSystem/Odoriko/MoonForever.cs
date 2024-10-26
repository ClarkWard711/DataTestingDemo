using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/MoonForever"), fileName = ("MoonForever"))]
public class MoonForever : OdorikoSkill
{
    public MoonForever()
    {
        SpCost = 10;
        odoSkillKind = OdoSkillKind.Moon;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.moonForever(SpCost, odoSkillKind));
    }
}
