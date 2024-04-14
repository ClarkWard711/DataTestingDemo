using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/SkillContainer"), fileName = ("SkillContainer_"))]

public class SkillContainer : ScriptableObject
{
    [SerializeField] List<Skill> SkillList;

    public List<Skill> skillList => SkillList;
}
