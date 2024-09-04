using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunRhythm"), fileName = ("SunRhythm"))]
public class SunRhythm : OdorikoSkill
{
    public SunRhythm()
    {
        SpCost = 8;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunRhythm(SpCost, odoSkillKind));
    }
}
