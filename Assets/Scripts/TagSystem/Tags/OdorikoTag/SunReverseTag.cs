using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunReverseTag : OdorikoTag
{
    public SunReverseTag()
    {
        odoTagKind = OdoTagKind.Sun;
        TagName = "SunReverseTag";
        TagKind = Kind.accumulable;
        quantity = 1;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        conditionMultiplier = 1.25f;
    }
}
