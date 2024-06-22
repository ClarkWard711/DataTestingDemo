using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFinaleTag : OdorikoTag
{
    public bool isCharged = false;
    public GameObject AtkUnit, DfsUnit;
    public SunFinaleTag()
    {
        odoTagKind = OdoTagKind.Sun;
        TagName = "SunFinaleTag";
        TagKind = Kind.turnLessen;
        TurnAdd = 1;
        TurnLast = 1;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        conditionMultiplier = 0.8f;
    }

    public override void OnTurnEndCallback()
    {
        int damage;
        damage = BattleSetting.Instance.DamageCountingByUnit(AtkUnit, DfsUnit, AttackType.Physical);
    }
}
