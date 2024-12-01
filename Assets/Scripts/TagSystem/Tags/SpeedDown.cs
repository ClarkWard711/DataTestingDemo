using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDown : Tag
{
    public SpeedDown()
    {
        TagName = "SpeedDown";
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
        spd = 0;
    }
}
