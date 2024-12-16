using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSAMSunTag : Tag
{
    public SOSAMSunTag()
    {
        TagName = "SOSAMSunTag";
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 1.15f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
    }
}
