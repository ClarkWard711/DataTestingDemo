using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OdorikoHolder : JobSkillHolder
{
    public static OdorikoHolder Instance;
    public bool MoonSpReduce = false, SunSpReduce = false;
    public float SpCostMultiplier = 1f;
    public bool LastTurnSun = false, LastTurnMoon = false, usedSkill = false;

    public override void Awake()
    {
        base.Awake();
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

    public IEnumerator sunSpot(int SpCost,OdoSkillKind odoSkillKind)
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
        StartCoroutine(BattleSetting.Instance.DealDamage(2f));
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

    public IEnumerator sunFinale(int SpCost,OdoSkillKind odoSkillKind)
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

    public IEnumerator sunReverse(int SpCost,OdoSkillKind odoSkillKind)
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
            
            BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical);
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
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Soul);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Physical);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Soul);
        }
        else
        {
            var temp = BattleSetting.Instance.CurrentActUnitTarget;
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
            yield return new WaitForSeconds(0.4f);
            BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, temp, AttackType.Soul);
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
        BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);

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
        BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);

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
                BattleSetting.Instance.DealDamageExtra(physicalDamage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
                yield return new WaitForSeconds(0.2f);
                BattleSetting.Instance.DealDamageExtra(physicalDamage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Soul);
            }
        }
        else
        {
            foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
            {
                BattleSetting.Instance.CurrentActUnitTarget = enemy;
                var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
                BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
            }
        }

        StartCoroutine(BattleSetting.Instance.ShowActionText("释放日轮"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator sunRose(int SpCost,OdoSkillKind odoSkillKind)
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.canChangeAction = false;
        DanceStepCheck(OdoSkillKind.Sun);

        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
        {
            
            if(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(Tag => Tag is OdorikoTag && ((OdorikoTag)Tag).odoTagKind == OdorikoTag.OdoTagKind.Moon))
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
        BattleSetting.Instance.StartCoroutine(BattleSetting.Instance.DealDamage(1f));
        
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
