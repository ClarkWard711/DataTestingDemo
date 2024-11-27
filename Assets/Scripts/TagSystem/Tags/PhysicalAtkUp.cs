using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAtkUp : Tag
{
    public PhysicalAtkUp()
    {
        TagName = "PhysicalAtkUp";
        Impact = impactOnMultiplier.PhysicalDeal;
        Multiplier = 1.2f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
    }
}
