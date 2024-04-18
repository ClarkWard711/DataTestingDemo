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
    public string Description;
    public virtual void Apply(GameObject unit)
    {
        BattleSetting.Instance.BasicPanel.SetActive(false);
        BattleSetting.Instance.BasicPanel.SetActive(false);
    }
}
