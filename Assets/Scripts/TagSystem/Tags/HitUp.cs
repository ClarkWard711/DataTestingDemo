using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitUp : Tag
{
    public HitUp()
    {
        TagName = "HitUp";
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
        hit = 0;
    }
}
