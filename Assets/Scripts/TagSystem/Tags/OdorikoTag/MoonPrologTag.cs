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

    }
}
