using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeFall : CsState
{
    public JadeFall()
    {
        TagName = "JadeFall";
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
                int HellingHp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.10f);
                player.GetComponent<GivingData>().CoroutineStart(player.GetComponent<GivingData>().FloatingHP(HellingHp));
            }
            else
            {
                int HellingHp = Mathf.CeilToInt(player.GetComponent<GivingData>().maxHP * 0.05f);
                player.GetComponent<GivingData>().CoroutineStart(player.GetComponent<GivingData>().FloatingHP(HellingHp));
                
            }
            remainTurn--;

            if(remainTurn == 0) {
                isEnhanced = false;
            }
        }
    }
}
