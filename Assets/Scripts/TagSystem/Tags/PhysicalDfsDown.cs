using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDfsDown : Tag
{
    public PhysicalDfsDown()
    {
        TagName = "PhysicalDfsDown";
        Impact = impactOnMultiplier.PhysicalTake;
        Multiplier = 1.2f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
