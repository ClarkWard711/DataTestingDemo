using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDisperse : BloodistSkill
{
    public BloodDisperse()
    {
        SpCost = 5;
        bloodistSkillKind = BloodistSkillKind.Blood;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        BloodistHolder.Instance.CoroutineStart(BloodistHolder.Instance.bloodDisperse(SpCost, bloodistSkillKind));
    }
}
