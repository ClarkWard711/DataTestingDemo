using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunProtection"), fileName = ("SunProtection"))]
public class SunProtection : OdorikoSkill
{
    public SunProtection()
    {
        SpCost = 15;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunProtection(SpCost, odoSkillKind));
    }
}
