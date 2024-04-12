using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Buff 
{
    public string BuffName;
    public bool isTriggered;
    public enum Kind {accumulable,turnLessen,eternal};
    public enum target {ally,enemy,all,self};
    public enum impactOnMultiplier {take,deal};
    public enum effect {good,bad,neutral};
    public impactOnMultiplier Impact;
    public Kind BuffKind;
    public target BuffTarget;
    public effect Effect;
    public int TurnLast;
    public int quantity;
    public float Multiplier;
    public Buff(Buff buff)
    {
        isTriggered = buff.isTriggered;
        BuffKind = buff.BuffKind;
        Impact = buff.Impact;
        BuffTarget = buff.BuffTarget;
        Effect = buff.Effect;
        TurnLast = buff.TurnLast;
        quantity = buff.quantity;
        Multiplier = buff.Multiplier;
    }
    public Buff(string Name, bool istriggerd, impactOnMultiplier impact, Kind buffkind, target bufftarget, effect effect, int turnLast, int Quantity, float multiplier)
    {
        BuffName = Name;
        isTriggered = istriggerd;
        Impact = impact;
        BuffKind = buffkind;
        BuffTarget = bufftarget;
        Effect = effect;
        TurnLast = turnLast;
        quantity = Quantity;
        Multiplier = multiplier;
    }
    public static Buff Defencing = new Buff("防御",true, impactOnMultiplier.take, Kind.turnLessen, target.self, effect.neutral, 1, 0, 0.8f);
    public static Buff Charging = new Buff("蓄力",true, impactOnMultiplier.deal, Kind.turnLessen, target.self, effect.neutral, 2, 0, 2f);
}
