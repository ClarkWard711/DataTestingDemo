using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAtkDown : Tag
{
    public SoulAtkDown()
    {
        TagName = "SoulAtkDown";
        Impact = impactOnMultiplier.SoulDeal;
        Multiplier = 0.8f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
