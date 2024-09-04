using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SunProtectionTag : OdorikoTag
{
    public SunProtectionTag()
    {
        odoTagKind = OdoTagKind.Sun;
        TagName = "SunProtection";
        TagKind = Kind.trigger;
        Effect = effect.good;
        BuffTarget = target.ally;
    }
}
