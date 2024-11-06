using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tag;
public class SoulDamage : CsState
{
    public SoulDamage()
    {
        TagName = "SoulDamage";
        Impact = impactOnMultiplier.SoulDeal;
        Multiplier = 1.05f;
        TagKind = Kind.turnLessen;
        BuffTarget = target.self;
        Effect = effect.neutral;
        TurnAdd = 1;
        TurnLast = 1;
    }
}
