using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearMoonTag : OdorikoTag
{
    public ClearMoonTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "ClearMoon";
        TagKind = Kind.turnLessen;
        TurnAdd = 3;
        TurnLast = 3;
        Effect = effect.good;
        BuffTarget = target.ally;
        Impact = impactOnMultiplier.AllDeal;
    }
    public override void OnActionEndCallback()
    {
        int deltaTemp;
        deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().maxHP * 0.15f);
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().CoroutineStart(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().FloatingHP(deltaTemp));
    }
}
