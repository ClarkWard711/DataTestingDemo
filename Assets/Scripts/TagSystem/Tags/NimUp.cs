using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimUp : Tag
{
    public NimUp()
    {
        TagName = "NimUp";
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.all;
        Effect = effect.good;
        TurnLast = 1;
        nim = 0;
    }
}
