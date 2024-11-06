using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliveryMoon : CsState
{
    public SliveryMoon()
    {
        TagName = "SliveryMoon";
        TagKind = Kind.eternal;
        Effect = effect.good;
        BuffTarget = target.self;
    }

    public override void OnTurnStartCallback()
    {
        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {
            if (isEnhanced)
            {
                int HellingHp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.06f);
                player.GetComponent<GivingData>().CoroutineStart
                    (player.GetComponent<GivingData>().FloatingHP(HellingHp));
                int HellingSp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxSP * 0.06f);
                player.GetComponent<GivingData>().CoroutineStart
                    (player.GetComponent<GivingData>().FloatingSP(HellingSp));
                // int PreDamage = BattleSetting.Instance.DamageCounting(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType);
                // AttackType attackType = AttackType.Soul;
                // int Damage = Mathf.CeilToInt((float)(PreDamage*1.1));
                // BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().takeDamage(Damage,attackType,false);
                var tag = SoulDamage.CreateInstance<SoulDamage>();
                tag.Multiplier = 1.10f;
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
                
            }
            else
            {
                int HellingHp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.03f);
                player.GetComponent<GivingData>().CoroutineStart
                    (player.GetComponent<GivingData>().FloatingHP(HellingHp));
                int HellingSp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxSP * 0.03f);
                player.GetComponent<GivingData>().CoroutineStart
                    (player.GetComponent<GivingData>().FloatingSP(HellingSp));
                // int PreDamage = BattleSetting.Instance.DamageCounting(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType);
                // AttackType attackType = AttackType.Soul;
                // int Damage = Mathf.CeilToInt((float)(PreDamage*1.05));
                // BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().takeDamage(Damage,attackType,false);
                var tag = SoulDamage.CreateInstance<SoulDamage>();
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
            }
            remainTurn--;

            if(remainTurn == 0) {
                isEnhanced = false;
            }
        }
    }
}
