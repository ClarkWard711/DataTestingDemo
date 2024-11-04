using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Tag;

public class DamageReduction : Tag
{
    public DamageReduction()
    {
        TagName = "DamageReduction";
        Impact = impactOnMultiplier.AllTake;
        Multiplier = 0.9f;
        TagKind = Kind.turnLessen;
        BuffTarget = target.self;
        Effect = effect.neutral;
        TurnAdd = 1;
        TurnLast = 1;
    }
}
