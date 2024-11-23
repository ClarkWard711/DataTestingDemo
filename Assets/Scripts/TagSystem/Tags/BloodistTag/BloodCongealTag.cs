using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCongealTag : Tag
{
    public BloodCongealTag()
    {
        TagName = "BloodConggealTag";
        spd = 0;
        TagKind = Kind.turnLessen;
        BuffTarget = target.enemy;
        Effect = effect.bad;
        TurnAdd = 1;
        TurnLast = 2;
    }
}
