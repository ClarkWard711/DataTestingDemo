using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/Moonlight"), fileName = ("MoonLight"))]
public class MoonLight : OdorikoSkill
{
    public MoonLight()
    {
        SpCost = 5;
        odoSkillKind = OdoSkillKind.Moon;
        SkillName = "月光";
    }
    
    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.moonLight(SpCost, odoSkillKind));
    }
}
