using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/CsSkill/Jadefall"), fileName = ("Jadefall"))]
public class Jadefall : CsSkill
{
    public Jadefall()
    {
        csSkillKind = CsSkillKind.Null;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        CelestialSeerHolder.Instance.CoroutineStart(CelestialSeerHolder.Instance.Jadefall(SpCost, csSkillKind));
    }
}
