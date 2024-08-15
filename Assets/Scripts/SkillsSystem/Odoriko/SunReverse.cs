using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/SunReverse"), fileName = ("SunReverse"))]

public class SunReverse : OdorikoSkill
{
    public SunReverse()
    {
        SpCost = 7;
        odoSkillKind = OdoSkillKind.Sun;
    }
}
