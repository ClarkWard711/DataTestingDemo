using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDamage : CsState
{
    public PhysicalDamage()
    {
        TagName = "PhysicalDamage";
        Impact = impactOnMultiplier.PhysicalDeal;
        Multiplier = 1.05f;
        TagKind = Kind.turnLessen;
        BuffTarget = target.self;
        Effect = effect.neutral;
        TurnAdd = 1;
        TurnLast = 1;
    }
}
