using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonlightTag : OdorikoTag
{
    public MoonlightTag()
    {
        odoTagKind = OdoTagKind.Sun;
        TagName = "MoonlightTag";
        TagKind = Kind.turnLessen;
        TurnLast = 2;
        TurnAdd = 2;
        Effect = effect.good;
        BuffTarget = target.ally;
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 1.2f;
    }
}
