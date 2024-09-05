using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunRound"), fileName = ("SunRound"))]
public class SunRound : OdorikoSkill
{
    public SunRound()
    {
        SpCost = 15;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunRound(SpCost, odoSkillKind));
    }
}
