using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OdorikoHolder : JobSkillHolder
{
    public static OdorikoHolder Instance;
    public bool MoonSpReduce = false, SunSpReduce = false;
    public float SpCostMultiplier = 1f;
    public bool LastTurnSun = false, LastTurnMoon = false, usedSkill = false;
    public bool isDanceStepTriggered = false;
    public bool canSpecialBeUsed = false;
    public bool isOnlyOnceUsed = false;
    public bool ignoreDanceStep = false;
    public int MoonUsedAmount = 0;
    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public override void AddSkillToButton()
    {
        base.AddSkillToButton();
        if (jobData.SpecialID != 0)
        {
            SpecialSkill.SetActive(true);
            SpecialButton.onClick.AddListener(() => JobSkill.skillList[jobData.SpecialID].Apply(BattleSetting.Instance.CurrentActUnit));
            SpecialSkill.GetComponentInChildren<Text>().text = JobSkill.skillList[jobData.SpecialID].SkillName;
            if (jobData.SpecialID == 16 && MoonUsedAmount >= 3)
            {
                canSpecialBeUsed = true;
            }
            else if (jobData.SpecialID == 17 || jobData.SpecialID == 18)
            {
                if (isDanceStepTriggered)
                {
                    canSpecialBeUsed = true;
                }
            }
            if (canSpecialBeUsed && !isOnlyOnceUsed && JobSkill.skillList[jobData.SpecialID].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
            {
                SpecialButton.interactable = true;
            }
            else
            {
                SpecialButton.interactable = false;
            }
        }
        else
        {
            SpecialSkill.SetActive(false);
        }
    }

    public void SpCounter(int SpCost, OdoSkillKind skillKind)
    {
        //base.SpCounter(SpCost);
        if (skillKind == OdoSkillKind.Moon)
        {
            MoonSpCost(SpCost);
        }
        else if (skillKind == OdoSkillKind.Sun)
        {
            SunSpCost(SpCost);
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
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
        MoonUsedAmount++;
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
        if (ignoreDanceStep)
        {
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(Charging.CreateInstance<Charging>());
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Find(tag => tag.TagName == "Charging").TurnLast--;
        }
        else
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
                    isDanceStepTriggered = true;
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
                    isDanceStepTriggered = true;
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
    }

    #region 基础
    public IEnumerator moonLight(int SpCost, OdoSkillKind odoSkillKind)
    {
        DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(MoonlightTag.CreateInstance<MoonlightTag>());
        BattleSetting.Instance.CheckTagList(BattleSetting.Instance.CurrentActUnit);
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
        BattleSetting.Instance.State = BattleState.Middle;
        StartCoroutine(BattleSetting.Instance.ShowActionText("月光"));

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(MoonlightTag.CreateInstance<MoonlightTag>());
            //到时动画播两遍
        }
        BattleSetting.Instance.canChangeAction = false;
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunSpot(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            MoonSpReduce = true;
            SpCostMultiplier = 0.8f;
        }
        else
        {
            MoonSpReduce = true;
            SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.GameStateText.text = "日斑";
        StartCoroutine(BattleSetting.Instance.ShowText(1f));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Physical;
        StartCoroutine(BattleSetting.Instance.DealDamage(2f, false));
    }
    #endregion

    #region Advanced
    public IEnumerator scarletMoon(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Moon);
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
            yield return new WaitForSeconds(1f);
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(ScarletMoon.CreateInstance<ScarletMoon>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放绯色月夜"));
            yield return new WaitForSeconds(1f);
        }

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
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
                StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放绯色月夜"));
                yield return new WaitForSeconds(1f);
            }
        }
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator fullMoon(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Moon);
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
            yield return new WaitForSeconds(1f);
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<FullMoonTag>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放盈月"));
            yield return new WaitForSeconds(1f);
        }

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
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
                yield return new WaitForSeconds(1f);
            }
            else
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<FullMoonTag>());
                StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放盈月"));
                yield return new WaitForSeconds(1f);
            }
        }
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator clearMoon(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Moon);
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
            yield return new WaitForSeconds(1f);
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<ClearMoonTag>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "清月"));
            yield return new WaitForSeconds(1f);
        }

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
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
                yield return new WaitForSeconds(1f);
            }
            else
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(FullMoonTag.CreateInstance<ClearMoonTag>());
                StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "清月"));
                yield return new WaitForSeconds(1f);
            }
        }
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator moonBless(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Moon);
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
            yield return new WaitForSeconds(1f);
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonBlessTag.CreateInstance<MoonBlessTag>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放月佑"));
            yield return new WaitForSeconds(1f);
        }

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
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
                yield return new WaitForSeconds(1f);
            }
            else
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonBlessTag.CreateInstance<MoonBlessTag>());
                StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放月佑"));
                yield return new WaitForSeconds(1f);
            }
        }
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator moonErode(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                enemy.GetComponent<GivingData>().AddTagToCharacter(MoonErodeTag.CreateInstance<MoonErodeTag>());
            }
            StartCoroutine(BattleSetting.Instance.ShowActionText("月蚀"));
            yield return new WaitForSeconds(1f);
        }
        else
        {
            foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(MoonErodeTag.CreateInstance<MoonErodeTag>());
                    }
                }
                else
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(MoonErodeTag.CreateInstance<MoonErodeTag>());
                    }
                }
            }
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放月蚀"));
            yield return new WaitForSeconds(1f);
        }

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
            if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
            {
                foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
                {
                    enemy.GetComponent<GivingData>().AddTagToCharacter(MoonErodeTag.CreateInstance<MoonErodeTag>());
                }
                StartCoroutine(BattleSetting.Instance.ShowActionText("月蚀"));
                yield return new WaitForSeconds(1f);
            }
            else
            {
                foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
                {
                    if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                        {
                            enemy.GetComponent<GivingData>().AddTagToCharacter(MoonErodeTag.CreateInstance<MoonErodeTag>());
                        }
                    }
                    else
                    {
                        if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                        {
                            enemy.GetComponent<GivingData>().AddTagToCharacter(MoonErodeTag.CreateInstance<MoonErodeTag>());
                        }
                    }
                }
                StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放月蚀"));
                yield return new WaitForSeconds(1f);
            }
        }
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator moonProlog(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(MoonPrologTagAdvanced.CreateInstance<MoonPrologTagAdvanced>());
                    }
                }
                else
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(MoonPrologTagAdvanced.CreateInstance<MoonPrologTagAdvanced>());
                    }
                }
            }
        }
        else
        {
            foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(MoonPrologTag.CreateInstance<MoonPrologTag>());
                    }
                }
                else
                {
                    if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        enemy.GetComponent<GivingData>().AddTagToCharacter(MoonPrologTag.CreateInstance<MoonPrologTag>());
                    }
                }
            }
        }
        StartCoroutine(BattleSetting.Instance.ShowActionText("月：序曲"));
        yield return new WaitForSeconds(1f);

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
            if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonPrologTagAdvanced.CreateInstance<MoonPrologTagAdvanced>());
            }
            else
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonPrologTag.CreateInstance<MoonPrologTag>());
            }
            StartCoroutine(BattleSetting.Instance.ShowActionText("月：序曲"));
            yield return new WaitForSeconds(1f);
        }
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator moonDuke(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Moon);
        SpCounter(SpCost, odoSkillKind);

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonDukeTag.CreateInstance<MoonDukeTag>());
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Find(Tag => Tag.TagName == "MoonDuke").TurnLast += 2;
            StartCoroutine(BattleSetting.Instance.ShowActionText("爵月"));
            yield return new WaitForSeconds(1f);
        }
        else
        {
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonDukeTag.CreateInstance<MoonDukeTag>());
            StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放爵月"));
            yield return new WaitForSeconds(1f);
        }

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "SunProtectionTag"))
        {
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.RemoveAll(Tag => Tag.TagName == "SunProtectionTag");
            if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonDukeTag.CreateInstance<MoonDukeTag>());
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Find(Tag => Tag.TagName == "MoonDuke").TurnLast += 2;
                StartCoroutine(BattleSetting.Instance.ShowActionText("爵月"));
                yield return new WaitForSeconds(1f);
            }
            else
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(MoonDukeTag.CreateInstance<MoonDukeTag>());
                StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放爵月"));
                yield return new WaitForSeconds(1f);
            }
        }
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunFinale(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            SunFinaleTag Tag = SunFinaleTag.CreateInstance<SunFinaleTag>();
            Tag.isCharged = true;
            Tag.AtkUnit = BattleSetting.Instance.CurrentActUnit;
            Tag.DfsUnit = BattleSetting.Instance.CurrentActUnitTarget;
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Tag);
        }
        else
        {
            SunFinaleTag Tag = SunFinaleTag.CreateInstance<SunFinaleTag>();
            Tag.AtkUnit = BattleSetting.Instance.CurrentActUnit;
            Tag.DfsUnit = BattleSetting.Instance.CurrentActUnitTarget;
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Tag);
        }
        StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget.name + "释放日：终曲"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunReverse(int SpCost, OdoSkillKind odoSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        List<GameObject> EnemiesToBeAttacked = new();
        foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
        {
            if (enemy.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
            {
                EnemiesToBeAttacked.Add(enemy);
            }
        }

        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {
            List<Tag> odorikoTags = player.GetComponent<GivingData>().tagList.FindAll(tag => tag is OdorikoTag && ((OdorikoTag)tag).odoTagKind == OdorikoTag.OdoTagKind.Moon);
            foreach (var tag in odorikoTags)
            {
                if (tag.TagKind == Tag.Kind.turnLessen)
                {
                    tag.TurnLast++;
                    if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
                    {
                        tag.TurnLast++;
                    }
                }
            }
        }

        foreach (var enemy in EnemiesToBeAttacked)
        {
            var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical);

            if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
            {
                damage = Mathf.CeilToInt(1.25f * damage);
            }

            BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical, false);
        }

        StartCoroutine(BattleSetting.Instance.ShowActionText("释放日溯"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunProtection(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            var temp = BattleSetting.Instance.CurrentActUnitTarget;
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Soul, false);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Physical, false);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Soul, false);
        }
        else
        {
            var temp = BattleSetting.Instance.CurrentActUnitTarget;
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Soul, false);
        }
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(SunProtectionTag.CreateInstance<SunProtectionTag>());
        StartCoroutine(BattleSetting.Instance.ShowActionText("释放日护"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunCurse(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
        damage = Mathf.CeilToInt(1.5f * damage);
        BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);

        var moonEnemyList = new List<GameObject>();
        foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
        {
            if (enemy.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
            {
                moonEnemyList.Add(enemy);
            }
        }

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            if (moonEnemyList.Count > 1)
            {
                var enemy = moonEnemyList[Random.Range(0, moonEnemyList.Count)];
                var tagList = enemy.GetComponent<GivingData>().tagList.FindAll(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon);
                moonEnemyList.Remove(enemy);
                var tag = tagList[Random.Range(0, tagList.Count)];
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);

                var secondEnemy = moonEnemyList[Random.Range(0, moonEnemyList.Count)];
                var secondTagList = secondEnemy.GetComponent<GivingData>().tagList.FindAll(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon);
                var secondTag = secondTagList[Random.Range(0, secondTagList.Count)];
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(secondTag);
            }
            else
            {
                if (moonEnemyList.Count != 0)
                {
                    var tagList = moonEnemyList[Random.Range(0, moonEnemyList.Count)].GetComponent<GivingData>().tagList.FindAll(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon);
                    var tag = tagList[Random.Range(0, tagList.Count)];
                    BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
                }
            }
        }
        else
        {
            if (moonEnemyList.Count != 0)
            {
                var tagList = moonEnemyList[Random.Range(0, moonEnemyList.Count)].GetComponent<GivingData>().tagList.FindAll(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon);
                var tag = tagList[Random.Range(0, tagList.Count)];
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
            }
        }

        StartCoroutine(BattleSetting.Instance.ShowActionText("释放日呪"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunRhythm(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
        BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            SunRhythmAdvancedTag tag = SunRhythmAdvancedTag.CreateInstance<SunRhythmAdvancedTag>();
            tag.atkUnit = BattleSetting.Instance.CurrentActUnit;
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
        }
        else
        {
            SunRhythmTag tag = SunRhythmTag.CreateInstance<SunRhythmTag>();
            tag.atkUnit = BattleSetting.Instance.CurrentActUnit;
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
        }

        StartCoroutine(BattleSetting.Instance.ShowActionText("释放日之律动"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunRound(int SpCost, OdoSkillKind odoSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                BattleSetting.Instance.CurrentActUnitTarget = enemy;
                var physicalDamage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
                var soulDamage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Soul);
                BattleSetting.Instance.DealDamageExtra(physicalDamage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
                yield return new WaitForSeconds(0.2f);
                BattleSetting.Instance.DealDamageExtra(soulDamage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Soul, false);
            }
        }
        else
        {
            foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                BattleSetting.Instance.CurrentActUnitTarget = enemy;
                var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
                BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
            }
        }

        StartCoroutine(BattleSetting.Instance.ShowActionText("释放日轮"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunRose(int SpCost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {

            if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
            {

            }
            else
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Attract.CreateInstance<Attract>());
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Bleed.CreateInstance<Bleed>());
            }
        }
        else
        {
            if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
            {

            }
            else
            {
                BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Attract.CreateInstance<Attract>());
            }
        }
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Physical;
        int damage = BattleSetting.Instance.DamageCounting(AttackType.Physical);
        damage = Mathf.CeilToInt(damage * 1.2f);
        BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);

        StartCoroutine(BattleSetting.Instance.ShowActionText("释放日瑰"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    /*public IEnumerator sunReverse(int Spcost, OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        OdorikoHolder.Instance.DanceStepCheck(OdoSkillKind.Sun);
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            SunFinaleTag Tag = SunFinaleTag.CreateInstance<SunFinaleTag>();
            Tag.isCharged = true;
            Tag.AtkUnit = BattleSetting.Instance.CurrentActUnit;
            Tag.DfsUnit = BattleSetting.Instance.CurrentActUnitTarget;
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Tag);
        }
        else
        {
            SunFinaleTag Tag = SunFinaleTag.CreateInstance<SunFinaleTag>();
            Tag.AtkUnit = BattleSetting.Instance.CurrentActUnit;
            Tag.DfsUnit = BattleSetting.Instance.CurrentActUnitTarget;
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Tag);
        }
        StartCoroutine(BattleSetting.Instance.ShowActionText("对" + BattleSetting.Instance.CurrentActUnitTarget + "释放日：终曲"));
        BattleSetting.Instance.ActionEnd();
    }*/
    #endregion

    #region Special
    public IEnumerator moonForever(int SpCost, OdoSkillKind odoSkillKind)
    {
        List<GameObject> players = new List<GameObject>();
        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {
            if (player.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
            {
                players.Add(player);
            }
        }
        if (players.Count != 0)
        {
            BattleSetting.Instance.canChangeAction = false;
            DanceStepCheck(OdoSkillKind.Moon);
            SpCounter(SpCost, odoSkillKind);
            MoonUsedAmount = 0;
            canSpecialBeUsed = false;
            SpecialButton.interactable = false;
            int i = Random.Range(0, players.Count);
            List<Tag> odorikoTags = players[i].GetComponent<GivingData>().tagList.FindAll(tag => tag is OdorikoTag && ((OdorikoTag)tag).odoTagKind == OdorikoTag.OdoTagKind.Moon);
            int index = Random.Range(0, odorikoTags.Count);
            odorikoTags[index].TagKind = Tag.Kind.eternal;
            //todo:对players[i]播放动画
            yield return new WaitForSeconds(1f);
            BattleSetting.Instance.ActionEnd();
        }
        else
        {
            StartCoroutine(BattleSetting.Instance.ShowActionText("无可作用的友方"));
        }
    }

    public IEnumerator myriads(int SpCost, OdoSkillKind odoSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        SpCounter(SpCost, odoSkillKind);
        SpecialButton.interactable = false;
        ignoreDanceStep = true;
        StartCoroutine(BattleSetting.Instance.ShowActionText("万象"));
        isOnlyOnceUsed = true;
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunlit(int SpCost, OdoSkillKind odoSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);
        SpCounter(SpCost, odoSkillKind);
        SpecialButton.interactable = false;
        isOnlyOnceUsed = true;
        StartCoroutine(BattleSetting.Instance.ShowActionText("日耀"));
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(SunlitTag.CreateInstance<SunlitTag>());
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();

    }

    public IEnumerator sunSpotOfSunlit(OdoSkillKind odoSkillKind)
    {
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            MoonSpReduce = true;
            SpCostMultiplier = 0.8f;
        }
        else
        {
            MoonSpReduce = true;
            SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.GameStateText.text = "日斑";
        StartCoroutine(BattleSetting.Instance.ShowText(1f));
        yield return new WaitForSeconds(0.5f);
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Physical;
        //StartCoroutine(BattleSetting.Instance.DealDamage(0.5f));
        BattleSetting.Instance.DealDamageWithNoCallBack(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
    }
    #endregion

    public override void ActionEndCallback()
    {
        if (!usedSkill)
        {
            LastTurnMoon = false;
            LastTurnSun = false;
        }
        /*if (usedSkill)
        {
            int i = BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().positionID;
            if (i <= 2)
            {
                PositionControl.Instance.PositionControlCore(i + 3);
            }
            else
            {
                PositionControl.Instance.PositionControlCore(i - 3);
            }
        }*/
        usedSkill = false;
        //换位置
        int i = BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().positionID;
        if (i <= 2)
        {
            PositionControl.Instance.PositionControlCore(i + 3);
        }
        else
        {
            PositionControl.Instance.PositionControlCore(i - 3);
        }
        base.ActionEndCallback();
    }
}
