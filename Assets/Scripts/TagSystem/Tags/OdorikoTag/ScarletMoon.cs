using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarletMoon : OdorikoTag
{
    public ScarletMoon()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "ScarletMoon";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllTake;
    }
}
