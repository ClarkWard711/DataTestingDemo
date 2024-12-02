using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriUp : Tag
{
    public CriUp()
    {
        TagName = "CriUp";
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
        cri = 0;
    }
}
