using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charging : Tag
{
    public Charging()
    {
        TagName = "Charging";
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 2f;
        TurnAdd = 2;
        TagKind = Kind.turnLessen;
        BuffTarget = target.self;
        Effect = effect.neutral;
        TurnLast = 2;
    }

    public override void OnActionEndCallback()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTurnEndCallback()
    {
        throw new System.NotImplementedException();
    }
}
