using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDfsDown : Tag
{
    public SoulDfsDown()
    {
        TagName = "SoulDfsDown";
        Impact = impactOnMultiplier.SoulTake;
        Multiplier = 1.2f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
