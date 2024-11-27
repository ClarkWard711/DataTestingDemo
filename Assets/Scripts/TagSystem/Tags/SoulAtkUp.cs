using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAtkUp : Tag
{
    public SoulAtkUp()
    {
        TagName = "SoulAtkUp";
        Impact = impactOnMultiplier.SoulDeal;
        Multiplier = 1.2f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
    }
}
