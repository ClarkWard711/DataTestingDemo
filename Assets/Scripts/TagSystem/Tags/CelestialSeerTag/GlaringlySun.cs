using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlaringlySun : CsState
{
  
    public GlaringlySun()
    {
        TagName = "GlaringlySun";
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
                int HellingHp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.12f);
                var tag = DamageReduction.CreateInstance<DamageReduction>();
                tag.Multiplier = 0.85f;
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
                player.GetComponent<GivingData>().CoroutineStart(player.GetComponent<GivingData>().FloatingHP(HellingHp));
            }
            else
            {
                var tag = DamageReduction.CreateInstance<DamageReduction>();
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
                int HellingHp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.08f);
                player.GetComponent<GivingData>().CoroutineStart(player.GetComponent<GivingData>().FloatingHP(HellingHp));
            }
            remainTurn--;

            if (remainTurn == 0)
            {
                isEnhanced = false;
            }
        }
    }
}