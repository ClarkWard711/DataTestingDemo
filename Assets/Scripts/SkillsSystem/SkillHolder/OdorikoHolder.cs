using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OdorikoHolder : JobSkillHolder
{
    public static OdorikoHolder Instance;
    public bool MoonSpReduce = false, SunSpReduce = false;
    public float SpCostMultiplier = 1f;
    bool LastTurnSun = false, LastTurnMoon = false, usedSkill = false;
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

    public void DanceStepCheck(OdoSkillKind skillKind)
    {
        usedSkill = true;
        if (skillKind == OdoSkillKind.Moon && BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
        {
            if (LastTurnSun)
            {
                LastTurnMoon = true;
                LastTurnSun = false;
                BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(Charging.CreateInstance<Charging>());
                BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Find(tag => tag.TagName == "Charging").TurnLast--;
            }
            else
            {
                LastTurnMoon = true;
            }
        }
        else if (skillKind == OdoSkillKind.Sun && BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee")) 
        {
            if (LastTurnMoon)
            {
                LastTurnSun = true;
                LastTurnMoon = false;
                BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(Charging.CreateInstance<Charging>());
                BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Find(tag => tag.TagName == "Charging").TurnLast--;
            }
            else
            {
                LastTurnSun = true;
            }
        }
        else
        {
            LastTurnMoon = false;
            LastTurnSun = false;
        }
    }

    public IEnumerator sunSpot(int SpCost,OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
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

    public IEnumerator scarletMoon(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        OdorikoHolder.Instance.DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee")) 
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee")) 
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(ScarletMoon.CreateInstance<ScarletMoon>());
                    }
                }
                else
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(ScarletMoon.CreateInstance<ScarletMoon>());
                    }
                }
            }
            StartCoroutine(BattleSetting.Instance.ShowActionText("绯色月夜"));
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(ScarletMoon.CreateInstance<ScarletMoon>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget + "释放绯色月夜"));
        }
    }

    public IEnumerator fullMoon(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        OdorikoHolder.Instance.DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            foreach (GameObject player in BattleSetting.Instance.RemainingPlayerUnits)
            {
                if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                {
                    if (player.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        player.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<FullMoonTag>());
                    }
                }
                else
                {
                    if (player.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                    { 
                        player.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<FullMoonTag>());
                    }
                }
            }
            StartCoroutine(BattleSetting.Instance.ShowActionText("盈月"));
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<FullMoonTag>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget + "释放盈月"));
        }
    }

    public IEnumerator clearMoon(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        OdorikoHolder.Instance.DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            foreach (GameObject player in BattleSetting.Instance.RemainingPlayerUnits)
            {
                if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                {
                    if (player.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        player.GetComponent<GivingData>().AddTagToCharacter(ClearMoonTag.CreateInstance<ClearMoonTag>());
                    }
                }
                else
                {
                    if (player.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        player.GetComponent<GivingData>().AddTagToCharacter(ClearMoonTag.CreateInstance<ClearMoonTag>());
                    }
                }
            }
            StartCoroutine(BattleSetting.Instance.ShowActionText("清月"));
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<ClearMoonTag>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget + "清月"));
        }
    }

    public IEnumerator moonBless(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        OdorikoHolder.Instance.DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            foreach (GameObject player in BattleSetting.Instance.RemainingPlayerUnits)
            {
                if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                {
                    if (player.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        player.GetComponent<GivingData>().AddTagToCharacter(MoonBlessTag.CreateInstance<MoonBlessTag>());
                    }
                }
                else
                {
                    if (player.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        player.GetComponent<GivingData>().AddTagToCharacter(MoonBlessTag.CreateInstance<MoonBlessTag>());
                    }
                }
            }
            StartCoroutine(BattleSetting.Instance.ShowActionText("月佑"));
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonBlessTag.CreateInstance<MoonBlessTag>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget + "释放月佑"));
        }
    }

    public override void ActionEndCallback()
    {
        if (!usedSkill)
        {
            LastTurnMoon = false;
            LastTurnSun = false;
        }
        usedSkill = false;
        base.ActionEndCallback();
    }
}
