using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/ClearMoon"), fileName = ("ClearMoon"))]
public class ClearMoon : OdorikoSkill
{
    public ClearMoon()
    {
        SpCost = 10;
        odoSkillKind = OdoSkillKind.Moon;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.clearMoon(SpCost, odoSkillKind));
    }
}
