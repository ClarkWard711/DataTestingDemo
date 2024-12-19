using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDown : Tag
{
    public HitDown()
    {
        TagName = "NimDown";
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
        hit = 0;
    }
}
