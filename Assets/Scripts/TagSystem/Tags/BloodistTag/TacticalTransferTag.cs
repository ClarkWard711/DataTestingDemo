using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalTransferTag : Tag
{
    public int damageToAlly;
    public bool isCharged = false;
    public TacticalTransferTag()
    {
        TagName = "TacticalTransfer";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.good;
        BuffTarget = target.self;
        Impact = impactOnMultiplier.AllDeal;
        Multiplier = 0.5f;
        BeforeHit += Transfer;
        OnHit += DealToAlly;
    }

    public void Transfer()
    {
        damageToAlly = BattleSetting.Instance.damageCache;
    }

    public void DealToAlly()
    {
        List<GameObject> otherPlayerList = new List<GameObject>();
        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {
            otherPlayerList.Add(player);
            otherPlayerList.Remove(BattleSetting.Instance.CurrentActUnitTarget);
        }

        var randomAlly = otherPlayerList[Random.Range(0, otherPlayerList.Count)];
        BattleSetting.Instance.DealDamageExtra(damageToAlly, BattleSetting.Instance.CurrentActUnitTarget, randomAlly, AttackType.Physical, true);

        if (isCharged)
        {
            int deltaTemp;
            deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().maxHP * 0.02f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().CoroutineStart(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().FloatingHP(deltaTemp));
        }
    }
}
