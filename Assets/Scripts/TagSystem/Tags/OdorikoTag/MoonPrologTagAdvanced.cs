using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonPrologTagAdvanced : OdorikoTag
{
    public MoonPrologTagAdvanced()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "MoonPrologAdvanced";
        TagKind = Kind.turnLessen;
        TurnAdd = 3;
        TurnLast = 3;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        conditionMultiplier = 1.5f;
        BeingHit += DealDamage;
    }

    public void DealDamage()
    {
        int damage;
        AttackType attackType;
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType == AttackType.Physical)
        {
            damage = BattleSetting.Instance.DamageCounting(AttackType.Soul);
            attackType = AttackType.Soul;
        }
        else
        {
            damage = BattleSetting.Instance.DamageCounting(AttackType.Physical);
            attackType = AttackType.Physical;
        }
        damage = Mathf.CeilToInt(damage * BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Find(Tag => Tag.TagName == "MoonPrologAdvanced").conditionMultiplier);
        BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.DealDamageBonus(damage, attackType));
    }
}
