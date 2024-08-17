using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonPrologTag : OdorikoTag
{
    public MoonPrologTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "MoonProlog";
        TagKind = Kind.turnLessen;
        TurnAdd = 3;
        TurnLast = 3;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        conditionMultiplier = 0.7f;
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
        damage = Mathf.CeilToInt(damage * BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Find(Tag => Tag.TagName == "MoonProlog").conditionMultiplier);
        BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.DealDamageBonus(damage, attackType));
    }
}
