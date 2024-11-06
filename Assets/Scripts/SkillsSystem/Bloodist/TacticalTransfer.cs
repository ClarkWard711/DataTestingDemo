using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalTransfer : BloodistSkill
{
    public TacticalTransfer()
    {
        SpCost = 6;
        bloodistSkillKind = BloodistSkillKind.other;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.tacticalTransfer(SpCost, bloodistSkillKind));
    }
}
