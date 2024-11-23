using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodcurseTag : Tag
{
    public BloodcurseTag()
    {
        TagName = "BloodCurseTag";
        spd = 0;
        TagKind = Kind.turnLessen;
        BuffTarget = target.self;
        Effect = effect.good;
        TurnAdd = 1;
        TurnLast = 2;
    }
}
