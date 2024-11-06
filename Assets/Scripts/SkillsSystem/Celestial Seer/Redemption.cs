using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/CsSkill/DawnShield"), fileName = ("DawnShield"))]
public class Redemption : CsSkill
{
    public Redemption()
    {
        SpCost = 20;
        csSkillKind = CsSkillKind.Null;
    }
    
    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        CelestialSeerHolder.Instance.CoroutineStart(CelestialSeerHolder.Instance.Redemption(SpCost, csSkillKind));
    }
}
