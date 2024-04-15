using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : Tag
{
    public Defense()
    {
        TagName = "Defense";
        Impact = impactOnMultiplier.PhysicalTake;
        Multiplier = 0.8f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.self;
        Effect = effect.neutral;
        TurnLast = 1;
    }
}
