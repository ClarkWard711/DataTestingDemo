using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamExtendAdvanced : Tag
{
    public GameObject unit;
    public DreamExtendAdvanced()
    {
        TagName = "DreamExtendAdvanced";
        TagKind = Kind.turnLessen;
        Effect = effect.good;
        TurnAdd = 3;
        TurnLast = 3;
        BuffTarget = target.self;
    }

    public override void OnTurnEndCallback()
    {
        unit.GetComponent<DreamlandKnightHolder>().DreamCount += 3;
    }
}
