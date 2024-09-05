using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/MoonProlog"), fileName = ("MoonProlog"))]

public class MoonProlog : OdorikoSkill
{
    public MoonProlog()
    {
        odoSkillKind = OdoSkillKind.Moon;
        SpCost = 15;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.moonProlog(SpCost, odoSkillKind));
    }
}
