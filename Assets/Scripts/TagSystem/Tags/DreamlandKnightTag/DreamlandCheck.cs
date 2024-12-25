using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamlandCheck : Tag
{
    public DreamlandCheck()
    {
        TagName = "DreamlandCheck";
        TagKind = Kind.special;
        Effect = effect.neutral;
        BuffTarget = target.self;
        BeforeHit += NormalAttackCheck;
    }

    public void NormalAttackCheck()
    {
        if(BattleSetting.Instance.isNormalAttack)
        {
            BattleSetting.Instance.damageCache = Mathf.CeilToInt(1 + 0.1f * DreamlandKnightHolder.Instance.DreamCount);
        }
    }
}
