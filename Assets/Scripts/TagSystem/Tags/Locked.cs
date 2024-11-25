using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locked : Tag
{
    public Locked()
    {
        TagName = "Locked";
        Multiplier = 1f;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.enemy;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
