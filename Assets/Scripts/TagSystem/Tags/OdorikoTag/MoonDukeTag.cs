using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonDukeTag : OdorikoTag
{
    public int DamageToSelf;
    private AttackType attackType;
    public MoonDukeTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "MoonDuke";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 0.75f;
        BeforeHit += DamageDistribute;
        OnHit += DealToSelf;
    }

    public void DamageDistribute()
    {
        int Damage = BattleSetting.Instance.DamageCounting(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType);
        attackType = BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType;
        DamageToSelf = Mathf.CeilToInt(Damage / 3f);
        Debug.Log(DamageToSelf);
        
    }

    public void DealToSelf()
    {
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().takeDamage(DamageToSelf, attackType, false);
    }
}
