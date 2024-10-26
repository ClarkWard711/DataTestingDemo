using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/Myriads"), fileName = ("Myriads"))]
public class Myriads : OdorikoSkill
{
    public Myriads()
    {
        SpCost = 8;
        odoSkillKind = OdoSkillKind.Null;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.myriads(SpCost, odoSkillKind));
    }
}
