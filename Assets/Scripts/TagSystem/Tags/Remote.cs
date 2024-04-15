using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote : Tag
{
    public Remote()
    {
        TagName = "Remote";
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 1f;
        TagKind = Kind.eternal;
        BuffTarget = target.self;
        Effect = effect.neutral;
    }
}
