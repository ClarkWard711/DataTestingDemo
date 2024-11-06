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
            if (CelestialSeerHolder.Instance.isEnhanced)
            {
                var tag = PhysicalDamage.CreateInstance<PhysicalDamage>();
                tag.Multiplier = 1.10f;
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
                var tag2 = SpeedUP.CreateInstance<SpeedUP>();
                tag2.spd = Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList
                    [player.GetComponent<GivingData>().jobData.JobLevel - 1].speed*0.12f);
                player.GetComponent<GivingData>().AddTagToCharacter(tag2);
            }
            else
            {
                var tag = PhysicalDamage.CreateInstance<PhysicalDamage>();
                player.GetComponent<GivingData>().AddTagToCharacter(tag);
                var tag2 = SpeedUP.CreateInstance<SpeedUP>();
                tag2.spd = Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList
                    [player.GetComponent<GivingData>().jobData.JobLevel - 1].speed*0.08f);
                player.GetComponent<GivingData>().AddTagToCharacter(tag2);
                
            }
            CelestialSeerHolder.Instance.remainTurn--;

            if(CelestialSeerHolder.Instance.remainTurn == 0) {
                CelestialSeerHolder.Instance.isEnhanced = false;
            }
        }
    }
}
