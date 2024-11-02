using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OdorikoTag;

public class BloodAddict : Tag
{
    public BloodAddict()
    {
        TagName = "BloodAddict";
        TagKind = Kind.eternal;
        Effect = effect.neutral;
        BuffTarget = target.all;
        OnSelfDamageTake += BloodAddictCheck;
    }

    public void BloodAddictCheck()
    {
        BloodistHolder.Instance.BloodAddictSelf++;
    }
}
