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
        conditionMultiplier = 0.5f;
        OnHit += CounterAttack;
    }

    public void CounterAttack()
    {
        //我方反击
        GameObject AtkUnit,DfsUnit;
        AtkUnit = BattleSetting.Instance.CurrentActUnitTarget;
        DfsUnit = BattleSetting.Instance.CurrentActUnit;
        int damage = BattleSetting.Instance.DamageCountingByUnit(AtkUnit,DfsUnit,AttackType.Physical);
        damage = Mathf.CeilToInt(damage * 0.5f);
        OdorikoHolder.Instance.StartCoroutine(BattleSetting.Instance.DealCounterDamage(damage, AttackType.Physical));
    }
}
