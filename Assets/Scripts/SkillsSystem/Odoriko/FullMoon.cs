using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/FullMoon"), fileName = ("FullMoon"))]
public class FullMoon : OdorikoSkill
{
    public FullMoon()
    {
        SpCost = 12;
        odoSkillKind = OdoSkillKind.Moon;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.fullMoon(SpCost, odoSkillKind));
    }
}
