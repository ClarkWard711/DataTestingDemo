using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunReverse"), fileName = ("SunReverse"))]

public class SunReverse : OdorikoSkill
{
    public SunReverse()
    {
        SpCost = 18;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        //OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunReverse(SpCost, odoSkillKind));
        OdorikoHolder.Instance.DanceStepCheck(OdoSkillKind.Sun);
        OdorikoHolder.Instance.SpCounter(SpCost, odoSkillKind);

        foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
        {
            var odorikoTagList = enemy.GetComponent<GivingData>().tagList.FindAll(tag => tag is OdorikoTag);

            if (odorikoTagList.Count == 0)
            {
                continue;
            }
        }
    }
}
