using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdorikoHolder : JobSkillHolder
{
    public static OdorikoHolder Instance;
    public bool MoonSpReduce = false, SunSpReduce = false;
    public float SpCostMultiplier = 1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SpCounter(int SpCost,OdoSkillKind skillKind)
    {
        //base.SpCounter(SpCost);
        if (skillKind == OdoSkillKind.Moon) 
        {
            MoonSpCost(SpCost);
        }
        else if(skillKind == OdoSkillKind.Sun)
        {
            SunSpCost(SpCost);
        }
    }

    void MoonSpCost(int SpCost)
    {
        if (MoonSpReduce)
        {
            MoonSpReduce = false;
            
        }
        else
        {
            SpCostMultiplier = 1f;
        }
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= Mathf.CeilToInt(SpCost * SpCostMultiplier);
    }

    void SunSpCost(int SpCost)
    {
        if (SunSpReduce)
        {
            SunSpReduce = false;
        }
        else
        {
            SpCostMultiplier = 1f;
        }
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= Mathf.CeilToInt(SpCost * SpCostMultiplier);
    }

    public void sunSpotCoroutine(int SpCost,OdoSkillKind odoSkillKind)
    {
        StartCoroutine(sunSpot(SpCost,odoSkillKind));
    }

    IEnumerator sunSpot(int SpCost,OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().BuffList.Exists(Buff => Buff.BuffName == "蓄力"))
        {
            SunSpReduce = true;
            SpCostMultiplier = 0.8f;
        }
        else
        {
            SunSpReduce = true;
            SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.GameStateText.text = "日斑";
        StartCoroutine(BattleSetting.Instance.ShowText(1f));
        StartCoroutine(BattleSetting.Instance.DealDamage(3f));
    }
}
