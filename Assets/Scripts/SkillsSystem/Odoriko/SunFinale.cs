using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunFinale"), fileName = ("SunFinale"))]

public class SunFinale : OdorikoSkill
{
    public SunFinale()
    {
        odoSkillKind = OdoSkillKind.Sun;
        SpCost = 10;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunFinale(SpCost, odoSkillKind));
    }
}
