using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDfsUp : Tag
{
    public PhysicalDfsUp()
    {
        TagName = "PhysicalDfsUp";
        Impact = impactOnMultiplier.PhysicalTake;
        Multiplier = 0.8f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
