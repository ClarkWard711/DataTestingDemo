using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/Sunlit"), fileName = ("Sunlit"))]
public class Sunlit : OdorikoSkill
{
    public Sunlit()
    {
        SpCost = 10;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunlit(SpCost, odoSkillKind));
    }
}
