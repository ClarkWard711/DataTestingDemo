using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBeat : CsState
{
    public StarBeat()
    {
        TagName = "StarBeat";
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
                var tag = PhysicalDamage.CreateInstance<PhysicalDamage>();
                tag.Multiplier = 1.10f;
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
                //TODO:加上速度
            }
            else
            {
                var tag = PhysicalDamage.CreateInstance<PhysicalDamage>();
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
                
            }
            remainTurn--;

            if(remainTurn == 0) {
                isEnhanced = false;
            }
        }
    }
}
