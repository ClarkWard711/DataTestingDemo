using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBlessTag : OdorikoTag
{
    public MoonBlessTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "MoonBless";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.good;
        BuffTarget = target.ally;
        Impact = impactOnMultiplier.AllDeal;
    }

    public override void OnTurnEndCallback()
    {
        //删除负面buff
    }
}
