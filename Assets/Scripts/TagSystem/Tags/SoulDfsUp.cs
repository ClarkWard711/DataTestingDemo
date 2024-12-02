using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDfsUp : Tag
{
    public SoulDfsUp()
    {
        TagName = "SoulDfsUp";
        Impact = impactOnMultiplier.SoulTake;
        Multiplier = 0.8f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
    }
}
