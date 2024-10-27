using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunlitTag : OdorikoTag
{
    public SunlitTag()
    {
        odoTagKind = OdoTagKind.Sun;
        TagName = "Sunlit";
        TagKind = Kind.eternal;
        Effect = effect.good;
        BuffTarget = target.ally;
    }

    public override void OnActionEndCallback()
    {
        if (OdorikoHolder.Instance.LastTurnMoon)
        {
            int TargetIndex = Random.Range(0, BattleSetting.Instance.RemainingEnemyUnits.Length);
            var randomEnemy = BattleSetting.Instance.RemainingEnemyUnits[TargetIndex];
            BattleSetting.Instance.CurrentActUnitTarget = randomEnemy;
            OdorikoHolder.Instance.StartCoroutine(OdorikoHolder.Instance.sunSpotOfSunlit(OdoSkillKind.Sun));
        }
    }
}
