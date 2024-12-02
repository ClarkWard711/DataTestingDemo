using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriDown : Tag
{
    public CriDown()
    {
        TagName = "CriDown";
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
        cri = 0;
    }
}
