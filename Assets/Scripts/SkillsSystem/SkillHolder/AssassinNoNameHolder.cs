using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AssassinNoNameHolder : JobSkillHolder
{
	public static AssassinNoNameHolder Instance;

	public int canMoveCount = 1;
	public int canMoveCountLimit = 1;
	public bool isRaidUsed;
	public bool isCollaborateUsed;
	GameObject RaidTarget;
	GameObject CollaborateTarget;
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
		var tag = CanMoveRefresh.CreateInstance<CanMoveRefresh>();
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
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
				//技能冷却中
				if (coolDownList[i] > 0)
				{
					AdvancedSkillButton[i].interactable = false;
				}
			}
		}
		if (jobData.SkillsID.Exists(num => num == 4))
		{
			if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote") && JobSkill.skillList[jobData.SkillsID[jobData.SkillsID.FindIndex(num => num == 4)]].SpCost <= BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP)
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 4)].interactable = true;
			}
			else
			{
				AdvancedSkillButton[jobData.SkillsID.FindIndex(num => num == 4)].interactable = false;
			}
		}
	}
	#region  基础
	public IEnumerator raid(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		isRaidUsed = true;
		RaidTarget = BattleSetting.Instance.CurrentActUnitTarget;
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			damage = Mathf.CeilToInt(damage * 2f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
        StartCoroutine(BattleSetting.Instance.OnDealDamage());
        var tag = SpeedDown.CreateInstance<SpeedDown>();   
        tag.spd = -Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().EnemyData.EnemyStatsList[BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().GetComponent<GivingData>().EnemyData.EnemyLevel - 1].speed * 0.1f);
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd++;
			tag.TurnLast++;
		}
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		BattleSetting.Instance.BattleUnitsList.Sort((x, y) => -x.GetComponent<GivingData>().Speed.CompareTo(y.GetComponent<GivingData>().Speed));
		StartCoroutine(BattleSetting.Instance.ShowActionText("偷袭"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator collaborate(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			BattleSetting.Instance.canChangeAction = false;
			BattleSetting.Instance.isChooseFinished = false;
			BattleSetting.Instance.State = BattleState.Middle;
			BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
			foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
			{
				var tag = SpeedUp.CreateInstance<SpeedUp>();
				if (!gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
				{
					tag.TurnAdd++;
					tag.TurnLast++;
				}
				tag.spd = Mathf.CeilToInt(player.GetComponent<GivingData>().jobData.JobStatsList[player.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f);
				player.GetComponent<GivingData>().AddTagToCharacter(tag);
			}
		}
		else
		{
			BattleSetting.Instance.isWaitForPlayerToChooseAlly = true;
			gameObject.GetComponent<Collider2D>().enabled = false;
			yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
			BattleSetting.Instance.canChangeAction = false;
			BattleSetting.Instance.isChooseFinished = false;
			BattleSetting.Instance.State = BattleState.Middle;
			BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
			gameObject.GetComponent<Collider2D>().enabled = true;
			var tag = SpeedUp.CreateInstance<SpeedUp>();
			tag.spd = Mathf.CeilToInt(BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().jobData.JobStatsList[BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f);
			if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
			{
				tag.TurnAdd++;
				tag.TurnLast++;
			}
			BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
			var tag0 = SpeedUp.CreateInstance<SpeedUp>();
			if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
			{
				tag0.TurnAdd++;
				tag0.TurnLast++;
			}
			tag0.spd = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().jobData.JobStatsList[gameObject.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].speed * 0.1f);
			gameObject.GetComponent<GivingData>().AddTagToCharacter(tag0);
		}
		BattleSetting.Instance.BattleUnitsList.Sort((x, y) => -x.GetComponent<GivingData>().Speed.CompareTo(y.GetComponent<GivingData>().Speed));
		StartCoroutine(BattleSetting.Instance.ShowActionText("协同"));
		isCollaborateUsed = true;
		CollaborateTarget = BattleSetting.Instance.CurrentActUnitTarget;
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
	#endregion

	#region Advanced
	public IEnumerator shadowFollow(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		var tag = PhysicalAtkUp.CreateInstance<PhysicalAtkUp>();
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd++;
			tag.TurnLast++;
		}
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("影随"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator shadowFrighten(int SpCost)
	{
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		var tag = PhysicalAtkDown.CreateInstance<PhysicalAtkDown>();
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd++;
			tag.TurnLast++;
		}
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("影吓"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator flashAttack(int SpCost)
	{
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		var tag = CriUp.CreateInstance<CriUp>();
		var tag0 = NimUp.CreateInstance<NimUp>();
		tag.cri = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().jobData.JobStatsList[gameObject.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].critical * 0.1f);
		tag0.nim = Mathf.CeilToInt(gameObject.GetComponent<GivingData>().jobData.JobStatsList[gameObject.GetComponent<GivingData>().GetComponent<GivingData>().jobData.JobLevel - 1].nimbleness * 0.1f);
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd++;
			tag.TurnLast++;
			tag0.TurnAdd++;
			tag0.TurnLast++;
		}
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag);
		gameObject.GetComponent<GivingData>().AddTagToCharacter(tag0);
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
		damage = Mathf.CeilToInt(damage * 0.8f);
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			damage = Mathf.CeilToInt(damage * 2f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
        StartCoroutine(BattleSetting.Instance.OnDealDamage());
        StartCoroutine(BattleSetting.Instance.ShowActionText("闪袭"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator preparation(int SpCost)
	{
		gameObject.GetComponent<Collider2D>().enabled = false;
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		gameObject.GetComponent<Collider2D>().enabled = true;
		var tag = Charging.CreateInstance<Charging>();
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd--;
			tag.TurnLast--;
		}
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		if (gameObject.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			var tag0 = Charging.CreateInstance<Charging>();
			if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
			{
				tag0.TurnAdd--;
				tag0.TurnLast--;
			}
			gameObject.GetComponent<GivingData>().AddTagToCharacter(tag0);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("伺机"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator sneak(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}

		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		float multiplier = 0.04f;
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			multiplier = 0.08f;
			if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
			{
				multiplier = 0.15f;
			}
		}
		int hp = Mathf.CeilToInt(multiplier * gameObject.GetComponent<GivingData>().maxHP);
		StartCoroutine(gameObject.GetComponent<GivingData>().FloatingHP(hp));
		int sp = Mathf.CeilToInt(multiplier * gameObject.GetComponent<GivingData>().maxSP);
		StartCoroutine(gameObject.GetComponent<GivingData>().FloatingSP(sp));
		for (int i = 0; i < 2; i++)
		{
			if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.Effect == Tag.effect.bad))
			{
				gameObject.GetComponent<GivingData>().tagList.Find(tag => tag.Effect == Tag.effect.bad && tag.TagKind == Tag.Kind.turnLessen).TurnLast--;
			}
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("潜行"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
	public IEnumerator shadowChange(int SpCost)
	{
		gameObject.GetComponent<Collider2D>().enabled = false;
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		gameObject.GetComponent<Collider2D>().enabled = true;
		var tag = ShadowChangeTag.CreateInstance<ShadowChangeTag>();
		var tag0 = ShadowChangeTag.CreateInstance<ShadowChangeTag>();
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd--;
			tag.TurnLast--;
			tag0.TurnAdd--;
			tag0.TurnLast--;
		}
		int difference = gameObject.GetComponent<GivingData>().Speed - BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().Speed;
		tag.spd = difference;
		tag0.spd = -difference;
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			gameObject.GetComponent<GivingData>().AddTagToCharacter(tag0);
		}
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("影移"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator launch(int SpCost)
	{
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
		damage = Mathf.CeilToInt(damage * 1.2f);
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			damage = Mathf.CeilToInt(damage * 2f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
        StartCoroutine(BattleSetting.Instance.OnDealDamage());
        yield return new WaitForSeconds(0.3f);
		if (isCollaborateUsed)
		{
			int damage0 = BattleSetting.Instance.DamageCountingByUnit(CollaborateTarget, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
			damage0 = Mathf.CeilToInt(damage0 * 1.2f);
			if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
			{
				damage0 = Mathf.CeilToInt(damage0 * 2f);
			}
			BattleSetting.Instance.DealDamageExtra(damage0, CollaborateTarget, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
		}
		StartCoroutine(BattleSetting.Instance.ShowActionText("出击"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator illusionFear(int SpCost)
	{
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
		damage = Mathf.CeilToInt(damage * 0.7f);
		var tag = Fear.CreateInstance<Fear>();
		tag.unit = BattleSetting.Instance.CurrentActUnitTarget;
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd++;
			tag.TurnLast++;
		}
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			damage = Mathf.CeilToInt(damage * 2f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
        StartCoroutine(BattleSetting.Instance.OnDealDamage());
        BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		yield return new WaitForSeconds(0.3f);
		if (isRaidUsed)
		{
			int damage0 = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, RaidTarget, AttackType.Physical);
			damage0 = Mathf.CeilToInt(damage0 * 0.7f);
			if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
			{
				damage0 = Mathf.CeilToInt(damage0 * 2f);
			}
			BattleSetting.Instance.DealDamageExtra(damage0, BattleSetting.Instance.CurrentActUnit, RaidTarget, AttackType.Physical, false);
		}
		var tag0 = Fear.CreateInstance<Fear>();
		tag0.unit = RaidTarget;
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag0.TurnAdd++;
			tag0.TurnLast++;
		}
		RaidTarget.GetComponent<GivingData>().AddTagToCharacter(tag0);
		StartCoroutine(BattleSetting.Instance.ShowActionText("幻惧"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator afterimage(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		AfterimageTag tag = AfterimageTag.CreateInstance<AfterimageTag>();
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.TurnAdd--;
			tag.TurnLast--;
		}
		if (gameObject.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			tag.mult = 0.2f;
		}
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("残影"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator bladeSwing(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		foreach (var enemy in BattleSetting.Instance.RemainingEnemyUnits)
        {
			if (enemy.GetComponent<GivingData>().tagList.Exists(tag => tag is Melee))
			{
				BattleSetting.Instance.CurrentActUnitTarget = enemy;
				var damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);

				if (gameObject.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
				{
					damage = Mathf.CeilToInt(damage * 1.4f);
				}
				else
				{
					damage = Mathf.CeilToInt(damage * 0.7f);
				}
				BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
                StartCoroutine(BattleSetting.Instance.OnDealDamage());
            }
		}
        StartCoroutine(BattleSetting.Instance.OnDealDamage());
        StartCoroutine(BattleSetting.Instance.ShowActionText("挥刃"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator poisonousBlade(int SpCost)
	{
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical);
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			damage = Mathf.CeilToInt(damage * 1.5f);
		}
		else
		{
			damage = Mathf.CeilToInt(damage * 0.8f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, BattleSetting.Instance.CurrentActUnitTarget, AttackType.Physical, false);
        StartCoroutine(BattleSetting.Instance.OnDealDamage());
        Poison tag = Poison.CreateInstance<Poison>();
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			tag.percentage = 0.02f;
		}
		else
		{
			tag.percentage = 0.04f;
		}
		tag.TurnLast++;
		tag.unit = BattleSetting.Instance.CurrentActUnitTarget;
		BattleSetting.Instance.CurrentActUnitTarget.GetComponent<GivingData>().AddTagToCharacter(tag);
		StartCoroutine(BattleSetting.Instance.ShowActionText("毒刺刃"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}

	public IEnumerator falseRetreat(int SpCost)
	{
		if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			SpCost = Mathf.CeilToInt(SpCost * 0.5f);
		}
		BattleSetting.Instance.canChangeAction = false;
		BattleSetting.Instance.isChooseFinished = false;
		BattleSetting.Instance.State = BattleState.Middle;
		BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
		if (!gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote"))
		{
			PositionControl.Instance.PositionControlCore(gameObject.GetComponent<GivingData>().positionID + 3);
		}
		var enemy = BattleSetting.Instance.RemainingEnemyUnits[Random.Range(0, BattleSetting.Instance.RemainingEnemyUnits.Length)];
		int damage = BattleSetting.Instance.DamageCountingByUnit(BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical);
		if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging"))
		{
			damage = Mathf.CeilToInt(damage * 0.8f);
		}
		else
		{
			damage = Mathf.CeilToInt(damage * 0.4f);
		}
		BattleSetting.Instance.DealDamageExtra(damage, BattleSetting.Instance.CurrentActUnit, enemy, AttackType.Physical, false);
        StartCoroutine(BattleSetting.Instance.OnDealDamage());
        StartCoroutine(BattleSetting.Instance.ShowActionText("佯装撤退"));
		yield return new WaitForSeconds(1f);
		BattleSetting.Instance.ActionEnd();
	}
	#endregion
	public override void ActionEndCallback()
	{
		if (gameObject.gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag.TagName == "Remote") && canMoveCount > 0)
		{
			canMoveCount--;
			BattleSetting.Instance.BattleUnitsList.Insert(0, gameObject);
			BattleSetting.Instance.BattleUnitsListToBeLaunched.RemoveAt(BattleSetting.Instance.BattleUnitsListToBeLaunched.FindIndex(obj => obj == gameObject));
		}
		else
		{
			isRaidUsed = false;
			isCollaborateUsed = false;
			RaidTarget = null;
			CollaborateTarget = null;
		}
		base.ActionEndCallback();
	}
}
