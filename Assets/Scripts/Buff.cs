using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Buff 
{
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
}
