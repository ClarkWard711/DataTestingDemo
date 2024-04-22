using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMoonTag : OdorikoTag
{
    public FullMoonTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "FullMoon";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.good;
        BuffTarget = target.ally;
        Impact = impactOnMultiplier.AllDeal;
    }
}
