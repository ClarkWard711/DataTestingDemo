using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonDukeTag : OdorikoTag
{
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
    }
}
