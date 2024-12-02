using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimDown : Tag
{
    public NimDown()
    {
        TagName = "NimDown";
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.bad;
        TurnLast = 1;
        nim = 0;
    }
}
