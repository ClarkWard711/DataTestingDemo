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
	public IEnumerator bloodDisperse(int SpCost, BloodistSkillKind bloodistSkillKind)
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
			int damage = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.04f);
			BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnit, AttackType.Physical, true);
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
		var tag = TacticalTransferTag.CreateInstance<TacticalTransferTag>();
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			tag.isCharged = true;
		}
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("战略转移"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator solo(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		int criTemp = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().cri * 0.1f);
		var tag1 = SoloTag.CreateInstance<SoloTag>();
		tag1.cri = criTemp;
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag1);
		gameObject.GetComponent<GivingData>().AddTagToCharacter(SoloTagDefense.CreateInstance<SoloTagDefense>());
		for (int i = 0; i < 3; i++)
		{
			BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
			//播放动画
			yield return new WaitForSeconds(0.1f);
			BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnitTarget, BattleSetting.Instance.CurrentActUnit, AttackType.Physical, false);
		}
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("独斗"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator noSelf(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		var tag = NoSelfCheck.CreateInstance<NoSelfCheck>();
		foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
		{
			BloodAddict bloodTag = (BloodAddict)player.GetComponent<GivingData>().tagList.Find(tag => tag.TagName == "BloodAddict");
			bloodTag.isNoSelf = true;
		}
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			tag.TurnLast += 1;
		}
		int id = jobData.SkillsID.FindIndex(id => id == 4);
		coolDownList[id] = 4;
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("无我"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodFight(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
		{
			Bleed bleedTag = Bleed.CreateInstance<Bleed>();
			enemy.GetComponent<GivingData>().AddTagToCharacter(bleedTag);
			bleedTag.unit = enemy;
			bleedTag.TurnLast += 2;
		}
		foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
		{
			Bleed bleedTag = Bleed.CreateInstance<Bleed>();
			player.GetComponent<GivingData>().AddTagToCharacter(bleedTag);
			bleedTag.unit = player;
			bleedTag.isSelf = true;
			bleedTag.TurnLast += 2;
			AllDamageUp tag = AllDamageUp.CreateInstance<AllDamageUp>();
			player.GetComponent<GivingData>().AddTagToCharacter(tag);
			tag.TurnLast += 2;
		}

		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{

		}

		StartCoroutine(BattleSetting.Instance.ShowActionText("无我"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
}
