using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillKind {Baic,Advanced,Special};
public class Skill : ScriptableObject
{
    public int SkillID;
    public int SpCost;
    public int TurnCooldown;
    public float DealMultiplier;
    public string SkillName;
    public virtual void Apply(GameObject unit)
    {

    }
}
