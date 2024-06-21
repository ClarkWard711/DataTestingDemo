using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonDukeTag : OdorikoTag
{
    public int DamageToSelf;
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
        BeforeHit += DamageDistribute;
        OnHit += DealToSelf;
    }

    public void DamageDistribute()
    {
        int Damage = BattleSetting.Instance.DamageCounting(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType);
        BattleSetting.Instance.TempDamage = Mathf.CeilToInt(Damage * 0.75f);
        DamageToSelf = Mathf.CeilToInt(Damage * 0.25f);
        
    }

    public void DealToSelf()
    {
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().takeDamage(DamageToSelf);
    }
}
