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
        TagKind = Kind.accumulable;
        quantity = 1;
        Effect = effect.bad;
        BuffTarget = target.enemy;
        Impact = impactOnMultiplier.AllDeal;
        conditionMultiplier = 0.8f;
    }

    public override void OnTurnEndCallback()
    {
        int physicalDamage,soulDamage;
        physicalDamage = Mathf.CeilToInt(quantity * conditionMultiplier * BattleSetting.Instance.DamageCountingByUnit(AtkUnit, DfsUnit, AttackType.Physical));
        soulDamage = Mathf.CeilToInt(quantity * conditionMultiplier * BattleSetting.Instance.DamageCountingByUnit(AtkUnit, DfsUnit, AttackType.Soul));

        if (isCharged)
        {
            int containMoonCount = 0;

            foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                var odorikoTagList = enemy.GetComponent<GivingData>().tagList.FindAll(tag => tag is OdorikoTag);

                if (odorikoTagList.Count == 0)
                {
                    continue;
                }
                containMoonCount++;
            }

            foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
            {
                var odorikoTagList = player.GetComponent<GivingData>().tagList.FindAll(tag => tag is OdorikoTag);

                if (odorikoTagList.Count == 0)
                {
                    continue;
                }
                containMoonCount++;
            }

            containMoonCount = Mathf.RoundToInt(containMoonCount / 2);

            if (isCharged)
            {
                BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.MethodActivateDelay(() => BattleSetting.Instance.DealDamageExtra(physicalDamage, AtkUnit, DfsUnit, AttackType.Physical), 0.1f, containMoonCount));
                BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.MethodActivateDelay(() => BattleSetting.Instance.DealDamageExtra(soulDamage, AtkUnit, DfsUnit, AttackType.Soul), 0.1f, containMoonCount));
            }
            else
            {
                BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.MethodActivateDelay(() => BattleSetting.Instance.DealDamageExtra(physicalDamage, AtkUnit, DfsUnit, AttackType.Physical), 0.1f, containMoonCount));
            }
        }

        quantity = 0;
        BattleSetting.Instance.CurrentEndTurnUnit.GetComponent<GivingData>().tagList.Remove(this);
    }
}
