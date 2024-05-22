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
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType == AttackType.Physical) 
        {
            damage = BattleSetting.Instance.DamageCounting(AttackType.Soul);
        }
        else
        {
            damage = BattleSetting.Instance.DamageCounting(AttackType.Physical);
        }
    }
}
