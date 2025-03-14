using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/CsSkill/RambleWithSky"), fileName = ("RambleWithSky"))]
public class RambleWithSky : CsSkill
{
    public RambleWithSky()
    {
        SpCost = 15;
        csSkillKind = CsSkillKind.Null;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        CelestialSeerHolder.Instance.CoroutineStart(CelestialSeerHolder.Instance.rambleWithSky(SpCost, csSkillKind));
    }
}
