using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = ("Skill/OdorikoSkill/Moonlight"), fileName = ("MoonLight"))]
public class MoonLight : OdorikoSkill
{
    public MoonLight()
    {
        odoSkillKind = OdoSkillKind.Moon;
    }
    
    public override void Apply(GameObject unit)
    {
        
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;

        OdorikoHolder.Instance.SpCounter(SpCost,odoSkillKind);
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(Defense.CreateInstance<Defense>());
        BattleSetting.Instance.CheckTagList(unit);
        if (unit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging")) 
        {
            OdorikoHolder.Instance.MoonSpReduce = true;
            OdorikoHolder.Instance.SpCostMultiplier = 0.8f;
        }
        else
        {
            OdorikoHolder.Instance.MoonSpReduce = true;
            OdorikoHolder.Instance.SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.State = BattleState.Middle;
        //StartCoroutine(BattleSetting.Instance.ShowActionText("月光"));
        
    }
}
