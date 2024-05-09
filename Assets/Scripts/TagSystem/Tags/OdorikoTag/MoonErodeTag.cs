using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonErodeTag : OdorikoTag
{
    public MoonErodeTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "MoonErode";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        OnHit += CounterAttack;
    }

    public void CounterAttack()
    {
        //我方反击
        GameObject temp;
        temp = BattleSetting.Instance.CurrentActUnitTarget;
        BattleSetting.Instance.CurrentActUnitTarget = BattleSetting.Instance.CurrentActUnit;
        BattleSetting.Instance.CurrentActUnit = temp;
        OdorikoHolder.Instance.StartCoroutine(BattleSetting.Instance.DealDamage(2f));
        BattleSetting.Instance.CurrentActUnit = BattleSetting.Instance.CurrentActUnitTarget;
        BattleSetting.Instance.CurrentActUnitTarget = temp;
    }
}
