using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalTransferTag : Tag
{
    public TacticalTransferTag()
    {
        TagName = "TacticalTransfer";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.good;
        BuffTarget = target.self;
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 0.8f;
    }

    public void Transfer()
    {

    }
}
