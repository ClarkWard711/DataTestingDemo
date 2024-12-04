using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowChangeTag : Tag
{
    public ShadowChangeTag()
    {
        TagName = "ShadowChangeTag";
        TurnAdd = 3;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 3;
        spd = 0;
    }
}
