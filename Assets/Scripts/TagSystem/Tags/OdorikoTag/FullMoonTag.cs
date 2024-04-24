using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMoonTag : OdorikoTag
{
    public FullMoonTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "FullMoon";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.good;
        BuffTarget = target.ally;
        Impact = impactOnMultiplier.AllDeal;
        OnHit += RestoreSp;
    }

    public void RestoreSp()
    {
        int deltaTemp;
        deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().maxSP * 0.1f);
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().CoroutineStart(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().FloatingSP(deltaTemp));
    }
}
