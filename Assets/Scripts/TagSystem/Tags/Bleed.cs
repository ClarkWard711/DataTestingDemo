using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : Tag
{
    public Bleed()
    {
        TagName = "Bleed";
        Impact = impactOnMultiplier.AllDeal;
        TurnAdd = 1;
        TagKind = Kind.turnLessen;
        BuffTarget = target.enemy;
        Effect = effect.bad;
        TurnLast = 1;
    }
}
