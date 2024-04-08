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
}
