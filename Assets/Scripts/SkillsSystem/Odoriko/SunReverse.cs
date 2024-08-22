using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunReverse"), fileName = ("SunReverse"))]

public class SunReverse : OdorikoSkill
{
    public SunReverse()
    {
        SpCost = 18;
        odoSkillKind = OdoSkillKind.Sun;
    }

    public override void Apply(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        base.Apply(unit);
        BattleSetting.Instance.State = BattleState.Middle;
        //OdorikoHolder.Instance.CoroutineStart(OdorikoHolder.Instance.sunReverse(SpCost, odoSkillKind));
        OdorikoHolder.Instance.DanceStepCheck(OdoSkillKind.Sun);
        OdorikoHolder.Instance.SpCounter(SpCost, odoSkillKind);
        List<GameObject> EnemiesToBeAttacked = new();
        foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
        {
            if (enemy.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag))
            {
                EnemiesToBeAttacked.Add(enemy);
            }
        }

        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits) 
        {
            List<Tag> odorikoTags = player.GetComponent<GivingData>().tagList.FindAll(tag => tag is OdorikoTag);
            foreach(var tag in odorikoTags)
            {
                if (tag.TagKind == Tag.Kind.turnLessen)
                {
                    tag.TurnLast++;
                    if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
                    {
                        tag.TurnLast++;
                    }
                }
            }
        }

        foreach (var enemy in EnemiesToBeAttacked)
        {
            var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical);

            if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
            {
                damage = Mathf.CeilToInt(1.25f * damage);
            }

            BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical);
        }
    }
}
