using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunRose"), fileName = ("SunRose"))]
public class SunRose : OdorikoSkill
{
    public SunRose()
    {
        SpCost = 8;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunRose(SpCost, odoSkillKind));
    }
}
