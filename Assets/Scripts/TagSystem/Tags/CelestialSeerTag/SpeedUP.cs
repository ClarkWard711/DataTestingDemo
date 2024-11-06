using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUP : CsState
{
    public SpeedUP()
    {
        TagName = "SpeedUP";
        spd = Mathf.CeilToInt(1.0f);
        TagKind = Kind.turnLessen;
        BuffTarget = target.self;
        Effect = effect.neutral;
        TurnAdd = 1;
        TurnLast = 1;
    }
}
