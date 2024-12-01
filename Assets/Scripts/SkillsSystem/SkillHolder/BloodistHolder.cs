using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		foreach (var player in BattleSetting.Instance.playerUnits)
		{
			player.GetComponent<GivingData>().AddTagToCharacter(BloodAddict.CreateInstance<BloodAddict>());
		}
	}

	public override void AddSkillToButton()
	{
		base.AddSkillToButton();
		for (int i = 0; i < 4; i++)
		{
			if (jobData.SkillsID[i] != -1)
			{
				//把按钮文字也给改了
				AdvancedSkillButton[i].GetComponentInChildren<Text>().text = JobSkill.skillList[jobData.SkillsID[i]].SkillName;
				BattleSetting.Instance.AdvancedPanel.GetComponentsInChildren<FloatingText>()[i].description = JobSkill.skillList[jobData.SkillsID[i]].Description;
				//AdvancedSkillButton[i].onClick.AddListener(() => JobSkill.skillList[jobData.SkillsID[i]].Apply(BattleSetting.Instance.CurrentActUnit));
				//写血咒的检测
				if (gameObject.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "BloodCurseTag"))
				{
					int allBloodAddict = (BloodAddictEnemy + BloodAddictSelf) * 3;
					if (JobSkill.skillList[jobData.SkillsID[i]].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP + allBloodAddict)
					{
						AdvancedSkillButton[i].interactable = false;
					}
					//技能冷却中
					if (coolDownList[i] > 0)
					{
						AdvancedSkillButton[i].interactable = false;
					}
				}
				else if (JobSkill.skillList[jobData.SkillsID[i]].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
				{
					AdvancedSkillButton[i].interactable = false;
				}
				//技能冷却中
				if (coolDownList[i] > 0)
				{
					AdvancedSkillButton[i].interactable = false;
				}
			}
		}
		if (jobData.SkillsID.Exists(num => num == 5))
		{
			if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee") && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 5)]].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 5)].interactable = true;
			}
			else
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 5)].interactable = false;
			}
		}
		if (jobData.SkillsID.Exists(num => num == 6))
		{
			if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote") && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 6)]].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 6)].interactable = true;
			}
			else
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 6)].interactable = false;
			}
		}
		if (jobData.SkillsID.Exists(num => num == 8))
		{
			if (GameObject.FindGameObjectsWithTag("Dead").Length != 0 && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 8)]].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 8)].interactable = true;
			}
			else
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 8)].interactable = false;
			}
		}
		if (jobData.SkillsID.Exists(num => num == 11))
		{
			if (gameObject.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "BloodCurseTag"))
			{
				int allBloodAddict = (BloodAddictEnemy + BloodAddictSelf) * 3;
				if (allBloodAddict > 14)
				{
					if (BloodAddictEnemy > BloodAddictSelf)
					{
						if (BloodAddictEnemy * 3 > 14)
						{
							if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee") && BloodAddictSelf >= 3 && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 11)]].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
							{
								AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = true;
							}
							else
							{
								AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = false;
							}
						}
						else
						{
							int SpCost = 14 - BloodAddictEnemy * 3;
							int count = 0;
							for (int i = 0; i * 3 < SpCost; i++)
							{
								count++;
							}
							if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee") && BloodAddictSelf - count >= 3 && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 11)]].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
							{
								AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = true;
							}
							else
							{
								AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = false;
							}
						}
					}
					else
					{
						if (BloodAddictSelf * 3 > 14)
						{
							int count = 0;
							for (int i = 0; i * 3 < 14; i++)
							{
								count++;
							}
							if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee") && BloodAddictSelf - count >= 3 && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 11)]].SpCost > BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
							{
								AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = true;
							}
							else
							{
								AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = false;
							}
						}
						else
						{
							AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = false;
						}
					}
				}
				else
				{
					AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = false;
				}
			}
			else if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee") && BloodAddictSelf >= 3 && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 11)]].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = true;
			}
			else
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 11)].interactable = false;
			}
		}
		if (jobData.SkillsID.Exists(num => num == 15))
		{
			if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is BloodInfectTag) && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 15)]].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 15)].interactable = true;
			}
			else
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 15)].interactable = false;
			}
		}
		if (jobData.SkillsID.Exists(num => num == 16))
		{
			if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote") && !gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is BloodFilthTag) && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 16)]].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 16)].interactable = true;
			}
			else
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 16)].interactable = false;
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (coolDownList[i] > 0)
			{
				AdvancedSkillButton[i].interactable = false;
			}
		}
	}
	public void CheckBloodCurseSP(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "BloodCurseTag"))
		{
			int allBloodAddict = (BloodAddictEnemy + BloodAddictSelf) * 3;
			if (allBloodAddict > SpCost)
			{
				if (BloodAddictEnemy > BloodAddictSelf)
				{
					if (BloodAddictEnemy * 3 > SpCost)
					{
						int count = 0;
						for (int i = 0; i * 3 < SpCost; i++)
						{
							count++;
						}
						BloodAddictEnemy -= count;
					}
					else
					{
						SpCost -= BloodAddictEnemy * 3;
						BloodAddictEnemy = 0;
						int count = 0;
						for (int i = 0; i * 3 < SpCost; i++)
						{
							count++;
						}
						BloodAddictSelf -= count;
					}
				}
				else
				{
					if (BloodAddictSelf * 3 > SpCost)
					{
						int count = 0;
						for (int i = 0; i * 3 < SpCost; i++)
						{
							count++;
						}
						BloodAddictSelf -= count;
					}
					else
					{
						SpCost -= BloodAddictSelf * 3;
						BloodAddictSelf = 0;
						int count = 0;
						for (int i = 0; i * 3 < SpCost; i++)
						{
							count++;
						}
						BloodAddictEnemy -= count;
					}
				}
			}
			else
			{
				SpCost -= allBloodAddict;
				BloodAddictEnemy = 0;
				BloodAddictSelf = 0;
				BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
			}
		}
		else
		{
			BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		}
	}
	#region 基础
	public IEnumerator bloodDisperse(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.DelimaPanel.SetActive(true);
		var choice1 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
		var choice2 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
		choice1.GetComponentInChildren<Text>().text = "造成伤害";
		choice2.GetComponentInChildren<Text>().text = "SP回复";
		//添加悬浮框显示文字
		choice1.GetComponent<Button>().onClick.AddListener(() => Choose(1));
		choice2.GetComponent<Button>().onClick.AddListener(() => Choose(2));
		yield return new WaitUntil(() => isChooseFin);
		isChooseFin = false;
		BattleSetting.Instance.DelimaPanel.SetActive(false);
		if (optionIndex == 1)
		{
			//伤害
			BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
			yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
			Destroy(choice1);
			Destroy(choice2);
			BattleSetting.Instance.canChangeAction = false;
			CheckBloodCurseSP(SpCost);
			BattleSetting.Instance.isChooseFinished = false;
			int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
			damage = Mathf.CeilToInt(damage * (1 + 0.2f * BloodAddictSelf));
			BloodAddictSelf = 0;
			BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
			yield return new WaitForSeconds(0.5f);
		}
		else if (optionIndex == 2)
		{
			//回复sp
			BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
			yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
			Destroy(choice1);
			Destroy(choice2);
			BattleSetting.Instance.canChangeAction = false;
			CheckBloodCurseSP(SpCost);
			BattleSetting.Instance.isChooseFinished = false;
			int deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().maxSP * 0.05f);
			StartCoroutine(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().FloatingSP(deltaTemp));
			int damage = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.04f);
			BattleSetting.Instance.DealDamageWithNoCallBack(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnit, AttackType.Physical, true);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("血散"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodAssemble(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.DelimaPanel.SetActive(true);
		var choice1 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
		var choice2 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
		choice1.GetComponentInChildren<Text>().text = "HP回复";
		choice2.GetComponentInChildren<Text>().text = "互斗";
		//添加悬浮框显示文字
		choice1.GetComponent<Button>().onClick.AddListener(() => Choose(1));
		choice2.GetComponent<Button>().onClick.AddListener(() => Choose(2));
		yield return new WaitUntil(() => isChooseFin);
		isChooseFin = false;
		BattleSetting.Instance.DelimaPanel.SetActive(false);
		if (optionIndex == 1)
		{
			BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
			yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
			Destroy(choice1);
			Destroy(choice2);
			BattleSetting.Instance.canChangeAction = false;
			CheckBloodCurseSP(SpCost);
			BattleSetting.Instance.isChooseFinished = false;
			int deltaTemp;
			deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().maxHP * (0.05f + 0.01f * BloodAddictEnemy));
			StartCoroutine(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().FloatingHP(deltaTemp));
			BloodAddictEnemy = 0;
		}
		else if (optionIndex == 2)
		{
			BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
			yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
			Destroy(choice1);
			Destroy(choice2);
			BattleSetting.Instance.canChangeAction = false;
			CheckBloodCurseSP(SpCost);
			BattleSetting.Instance.isChooseFinished = false;
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
		CheckBloodCurseSP(SpCost);
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
		CheckBloodCurseSP(SpCost);
		int criTemp = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().cri * 0.1f);
		var tag1 = SoloTag.CreateInstance<SoloTag>();
		tag1.cri = criTemp;
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag1);
		gameObject.GetComponent<GivingData>().AddTagToCharacter(SoloTagDefense.CreateInstance<SoloTagDefense>());
		for (int i = 0; i < 3; i++)
		{
			BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
			//播放动画
			yield return new WaitForSeconds(0.3f);
			BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnitTarget, BattleSetting.Instance.CurrentActUnit, AttackType.Physical, false);
		}
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			yield return new WaitForSeconds(0.3f);
			BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		}
		yield return new WaitForSeconds(0.3f);
		BattleSetting.Instance.CurrentActUnit = this.gameObject;
		StartCoroutine(BattleSetting.Instance.ShowActionText("独斗"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator noSelf(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
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
		CheckBloodCurseSP(SpCost);
		foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
		{
			Bleed bleedTag = Bleed.CreateInstance<Bleed>();
			bleedTag.unit = enemy;
			bleedTag.TurnAdd += 2;
			bleedTag.TurnLast += 2;
			enemy.GetComponent<GivingData>().AddTagToCharacter(bleedTag);
		}
		foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
		{
			Bleed bleedTag = Bleed.CreateInstance<Bleed>();
			bleedTag.unit = player;
			bleedTag.isSelf = true;
			bleedTag.TurnAdd += 2;
			bleedTag.TurnLast += 2;
			player.GetComponent<GivingData>().AddTagToCharacter(bleedTag);
			AllDamageUp tag = AllDamageUp.CreateInstance<AllDamageUp>();
			tag.TurnAdd += 2;
			tag.TurnLast += 2;
			player.GetComponent<GivingData>().AddTagToCharacter(tag);
		}

		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{

		}

		StartCoroutine(BattleSetting.Instance.ShowActionText("浴血奋战"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator clamour(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		var tag = Protect.CreateInstance<Protect>();
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			tag.TurnAdd++;
			tag.TurnLast++;
		}
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("叫嚣"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodThirst(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		for (int i = 0; i < 3; i++)
		{
			var enemy = BattleSetting.Instance.RemainingEnemyUnits[Random.Range(0, BattleSetting.Instance.RemainingEnemyUnits.Length)];

			BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical, false);
			int damage = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.08f);
			BattleSetting.Instance.DealDamageWithNoCallBack(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnit, AttackType.Physical, true);
			yield return new WaitForSeconds(0.2f);
		}
		int id = jobData.SkillsID.FindIndex(id => id == 7);
		coolDownList[id] = 2;
		StartCoroutine(BattleSetting.Instance.ShowActionText("嗜血"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodAlly(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		int bloodAddictBonus = BloodAddictEnemy >= 3 ? 3 : BloodAddictEnemy;
		int deltaTemp;
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().maxHP * (0.1f + bloodAddictBonus * 0.05f));
			BloodAddictEnemy -= bloodAddictBonus;
		}
		else
		{
			deltaTemp = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().maxHP * 0.1f);
		}
		BattleSetting.Instance.CurrentActUnitTarget.tag = "PlayerUnit";
		BattleSetting.Instance.RemainingPlayerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");

		StartCoroutine(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().FloatingHP(deltaTemp));
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().isDead = false;
		BattleSetting.Instance.CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(255, 255, 255, 1);
		int damage = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.05f);
		BattleSetting.Instance.DealDamageWithNoCallBack(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnit, AttackType.Physical, true);
		yield return new WaitForSeconds(0.3f);
		StartCoroutine(BattleSetting.Instance.ShowActionText("血盟"));
		yield return new WaitForSeconds(0.6f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodRain(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
		{
			BattleSetting.Instance.CurrentActUnitTarget = enemy;
			var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
			damage = Mathf.CeilToInt(damage * 0.8f);
			BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		}
		yield return new WaitForSeconds(0.5f);
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
		{
			int deltaTemp = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.02f * BloodAddictSelf);
			StartCoroutine(gameObject.GetComponent<GivingData>().FloatingHP(deltaTemp));
			BloodAddictSelf = 0;
		}
		else
		{
			while (BloodAddictEnemy > 0)
			{
				if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.Effect == Tag.effect.bad))
				{
					gameObject.GetComponent<GivingData>().tagList.Find(tag => tag.Effect == Tag.effect.bad && tag.TagKind != Tag.Kind.eternal).TurnLast--;
					BloodAddictEnemy--;
				}
				else
				{
					break;
				}
			}
		}
		int id = jobData.SkillsID.FindIndex(id => id == 9);
		coolDownList[id] = 2;
		StartCoroutine(BattleSetting.Instance.ShowActionText("血雨"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodMourning(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		int count = BloodAddictEnemy >= 3 ? 3 : BloodAddictEnemy;
		for (int i = 0; i < count + 1; i++)
		{
			int randomNum = Random.Range(0, 6);
			if (randomNum == 0)
			{
				int damage = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.08f);
				BattleSetting.Instance.DealDamageWithNoCallBack(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnit, AttackType.Physical, true);
			}
			else if (randomNum == 1)
			{
				foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
				{
					BattleSetting.Instance.CurrentActUnitTarget = enemy;
					var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
					damage = Mathf.CeilToInt(damage * 0.75f);
					BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
				}
			}
			else if (randomNum == 2)
			{
				var enemy = BattleSetting.Instance.RemainingEnemyUnits[Random.Range(0, BattleSetting.Instance.RemainingEnemyUnits.Length)];
				BattleSetting.Instance.DealDamageExtra(-1, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical, false);
			}
			else if (randomNum == 3)
			{
				var player = BattleSetting.Instance.RemainingEnemyUnits[Random.Range(0, BattleSetting.Instance.RemainingPlayerUnits.Length)];
				var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, player, AttackType.Physical);
				damage = Mathf.CeilToInt(damage * 0.5f);
				BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, player, AttackType.Physical, true);
			}
			else if (randomNum == 4)
			{
				//int Sp = BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP <= 10 ? 10 : BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP;
				//BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= Sp;
				//StartCoroutine(BattleSetting.Instance.ShowActionText("SP减少"));
				StartCoroutine(gameObject.GetComponent<GivingData>().FloatingSP(10));
				yield return new WaitForSeconds(0.2f);
			}
			else if (randomNum == 5)
			{
				int deltaTemp = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().maxHP * 0.08f);
				StartCoroutine(gameObject.GetComponent<GivingData>().FloatingHP(deltaTemp));
			}

			yield return new WaitForSeconds(0.2f);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("血殇"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodCongeal(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		BloodAddictSelf -= 3;
		var tag = BloodCongealTag.CreateInstance<BloodCongealTag>();
		tag.TurnAdd++;
		tag.spd = -BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().Speed;
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("血凝"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodCurse(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		var tag = BloodcurseTag.CreateInstance<BloodcurseTag>();
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("血咒"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodStab(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		var damage = Mathf.CeilToInt(1.2f * BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical));
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Locked.CreateInstance<Locked>());
		yield return new WaitForSeconds(0.3f);
		if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.name == "Melee"))
		{
			if (BloodAddictSelf >= 2 && BattleSetting.Instance.EnemyPositionsList[BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().positionID - 3].transform.childCount != 0)
			{
				BloodAddictSelf -= 2;
				BattleSetting.Instance.CurrentActUnitTarget = BattleSetting.Instance.EnemyPositionsList[BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().positionID - 3].transform.GetChild(0).gameObject;
				var damage1 = Mathf.CeilToInt(1.2f * BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical));
				BattleSetting.Instance.DealDamageExtra(damage1, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
				BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(Locked.CreateInstance<Locked>());
			}
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("血刺"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodMonogatari(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		int count = 0;
		if (BloodAddictSelf >= 4)
		{
			if (BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().tagList.Exists(tag => tag.name == "Melee"))
			{
				Fear tag = Fear.CreateInstance<Fear>();
				tag.unit = BattleSetting.Instance.CurrentActUnitTarget;
				BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
			else
			{
				Attract tag = Attract.CreateInstance<Attract>();
				tag.unit = BattleSetting.Instance.CurrentActUnitTarget;
				BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
			count++;
		}
		if (BloodAddictSelf >= 3)
		{
			var tag = Weak.CreateInstance<Weak>();
			tag.TurnAdd = 2;
			tag.TurnLast = 2;
			BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			count++;
		}
		if (BloodAddictSelf >= 2)
		{
			var tag = Bleed.CreateInstance<Bleed>();
			tag.TurnAdd = 2;
			tag.TurnLast = 2;
			BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			count++;
		}
		if (BloodAddictSelf >= 1)
		{
			var tag = PhysicalDfsDown.CreateInstance<PhysicalDfsDown>();
			tag.TurnAdd = 2;
			tag.TurnLast = 2;
			BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			count++;
		}
		BloodAddictSelf -= count;
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			var unit = BattleSetting.Instance.CurrentActUnitTarget;
			foreach (GameObject enemy in BattleSetting.Instance.RemainingEnemyUnits)
			{
				if (unit.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
				{
					if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Melee"))
					{
						var damage = Mathf.CeilToInt(1.2f * BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical));
						BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical, false);
					}
				}
				else
				{
					if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
					{
						var damage = Mathf.CeilToInt(1.2f * BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical));
						BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical, false);
					}
				}
			}
		}
		else
		{
			var damage = Mathf.CeilToInt(1.2f * BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical));
			BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("血咒"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodInfect(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		BloodInfectTag tag = BloodInfectTag.CreateInstance<BloodInfectTag>();
		tag.tagPosID = Random.Range(0, 6);
		tag.unit = this.gameObject;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(tag);
		List<int> targetID = BattleSetting.Instance.CheckSurroundPosition(tag.tagPosID);
		foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
		{
			if (targetID.Exists(id => id == enemy.GetComponent<GivingData>().positionID) || enemy.GetComponent<GivingData>().positionID == tag.tagPosID)
			{
				var damage = Mathf.CeilToInt(0.3f * BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical));
				BattleSetting.Instance.DealDamageWithNoCallBack(damage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical, false);
			}
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("血染"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bloodFilth(int SpCost, BloodistSkillKind bloodistSkillKind)
	{
		BattleSetting.Instance.canChangeAction = false;
		CheckBloodCurseSP(SpCost);
		BloodFilthTag tag = BloodFilthTag.CreateInstance<BloodFilthTag>();
		int count = BloodAddictEnemy >= 5 ? 5 : BloodAddictEnemy;
		tag.percentage = count * 0.2f;
		tag.unit = this.gameObject;
		BloodAddictEnemy -= count;
		tag.BeingHitCount = 2;
		tag.HitCount = 2;
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			tag.BeingHitCount++;
			tag.HitCount++;
		}
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("血秽"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
}
