using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Tag
{
    public Melee()
    {
        TagName = "Melee";
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 1f;
        TagKind = Kind.eternal;
        BuffTarget = target.self;
        Effect = effect.neutral;
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
