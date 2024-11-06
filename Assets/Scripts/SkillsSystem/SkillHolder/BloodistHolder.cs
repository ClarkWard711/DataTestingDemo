using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodistHolder : JobSkillHolder
{
    public static BloodistHolder Instance;
    public int optionIndex = 0;
    public bool isChooseFin = false;
    public int BloodAddictEnemy = 0, BloodAddictSelf = 0;

    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        var tag = BloodAddict.CreateInstance<BloodAddict>();
        tag.isBloodist = true;
        gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {
            player.GetComponent<GivingData>().AddTagToCharacter(BloodAddict.CreateInstance<BloodAddict>());
        }
    }

    #region 基础
    public IEnumerator bloodDisperse(int SpCost,BloodistSkillKind bloodistSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
        BattleSetting.Instance.DelimaPanel.SetActive(true);
        var choice1 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
        var choice2 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
        choice1.GetComponentInChildren<Text>().text = "造成伤害";
        choice2.GetComponentInChildren<Text>().text = "SP回复";
        //添加悬浮框显示文字
        choice1.GetComponent<Button>().onClick.AddListener(() => Choose(1));
        choice2.GetComponent<Button>().onClick.AddListener(() => Choose(2));
        yield return new WaitUntil(() => isChooseFin);
        Destroy(choice1);
        Destroy(choice2);
        isChooseFin = false;
        BattleSetting.Instance.DelimaPanel.SetActive(false);
        if (optionIndex == 1)
        {
            //伤害
            BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
            yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
            int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
            damage = Mathf.CeilToInt(damage * (1 + 0.2f * BloodAddictSelf));
            BloodAddictSelf = 0;
            BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
        }
        else if (optionIndex == 2) 
        {
            //回复sp
            BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
            yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
            int deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().maxSP * 0.05f);
            StartCoroutine(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().FloatingSP(deltaTemp));
            int damage= Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.04f);
            BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().takeDamage(damage, AttackType.Physical, true);
        }
        StartCoroutine(BattleSetting.Instance.ShowActionText("血散"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public IEnumerator bloodAssemble(int SpCost, BloodistSkillKind bloodistSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
        BattleSetting.Instance.DelimaPanel.SetActive(true);
        var choice1 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
        var choice2 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
        choice1.GetComponentInChildren<Text>().text = "HP回复";
        choice2.GetComponentInChildren<Text>().text = "互斗";
        //添加悬浮框显示文字
        choice1.GetComponent<Button>().onClick.AddListener(() => Choose(1));
        choice2.GetComponent<Button>().onClick.AddListener(() => Choose(2));
        yield return new WaitUntil(() => isChooseFin);
        Destroy(choice1);
        Destroy(choice2);
        isChooseFin = false;
        BattleSetting.Instance.DelimaPanel.SetActive(false);
        if (optionIndex == 1)
        {
            BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
            yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
            int deltaTemp;
            deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().maxHP * (0.05f + 0.01f * BloodAddictEnemy));
            StartCoroutine(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().FloatingHP(deltaTemp));
            BloodAddictEnemy = 0;
        }
        else if (optionIndex == 2)
        {
            BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
            yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
            int damage = BattleSetting.Instance.DamageCounting(AttackType.Physical);
            damage = Mathf.CeilToInt(1.5f * damage);
            BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
            BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().takeDamage(1, AttackType.Physical, false);
        }
        StartCoroutine(BattleSetting.Instance.ShowActionText("血聚"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }
    /// <summary>
    /// 抉择
    /// </summary>
    /// <param name="optionNum"></param>
    public void Choose(int optionNum)
    {
        optionIndex = optionNum;
        isChooseFin = true;
    }
    #endregion

    public IEnumerator tacticalTransfer(int SpCost, BloodistSkillKind bloodistSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
        gameObject.GetComponent<GivingData>().AddTagToCharacter(TacticalTransferTag.CreateInstance<TacticalTransferTag>());
        StartCoroutine(BattleSetting.Instance.ShowActionText("战略转移"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }
}
