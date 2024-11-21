using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonBlessTag : OdorikoTag
{
    public MoonBlessTag()
    {
        odoTagKind = OdoTagKind.Moon;
        TagName = "MoonBless";
        TagKind = Kind.turnLessen;
        TurnAdd = 2;
        TurnLast = 2;
        Effect = effect.good;
        BuffTarget = target.ally;
        Impact = impactOnMultiplier.AllDeal;
    }

    public override void OnTurnEndCallback()
    {
        // 只能删除一些bad的tag 例如永久的则无法删除
        List<Tag> badTags = new List<Tag>();
        foreach (Tag tag in BattleSetting.Instance.CurrentEndTurnUnit.GetComponent<GivingData>().tagList)
        {
            if (tag.Effect == effect.bad && tag.TagKind != Kind.eternal)
            {
                badTags.Add(tag);
            }
        }
        if (badTags.Count > 0)
        {
            int randomIndex = Random.Range(0, badTags.Count);
            BattleSetting.Instance.CurrentEndTurnUnit.GetComponent<GivingData>().tagList.RemoveAt(randomIndex);
        }
    }
}
