using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAtkDown : Tag
{
    public PhysicalAtkDown()
    {
        TagName = "PhysicalAtkDown";
        Impact = impactOnMultiplier.PhysicalDeal;
        Multiplier = 0.8f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
