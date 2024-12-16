using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOSAMSunTagSecond : Tag
{
    public SOSAMSunTagSecond()
    {
        TagName = "SOSAMSunTag";
        Impact = impactOnMultiplier.AllTake;
        Multiplier = 1.15f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
