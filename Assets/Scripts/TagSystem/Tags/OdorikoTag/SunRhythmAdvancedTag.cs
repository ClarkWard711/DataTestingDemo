using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRhythmAdvancedTag : OdorikoTag
{
    public GameObject atkUnit;
    public SunRhythmAdvancedTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "SunRhythmAdvanced";
        TagKind = Kind.turnLessen;
        TurnAdd = 3;
        TurnLast = 3;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        conditionMultiplier = 0.5f;
        BeingHit += DealDamage;
    }

    public void DealDamage()
    {
        int damage = Mathf.CeilToInt(conditionMultiplier * BattleSetting.Instance.DamageCountingByUnit(atkUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical));
        BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.DealDamageBonus(damage, AttackType.Physical));
    }
}
