using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum AttackType { Physical, Soul, Null };
public class GivingData : MonoBehaviour
{
	public JobData jobData;
	public EnemyData EnemyData;
	public int Speed, currentHP, currentSP, pa, sa, pd, sd, hit, nim, cri, melee, remote, maxHP, maxSP, AntiCri, physicalDizziness, soulDizziness, pAntiDizziness, sAntiDizziness, expPerlevel, sxpPerlevel, exp, sxp, dizzinessBar;
	public int positionID;
	public AttackType attackType = AttackType.Null;
	public bool isDead = false;
	//public Text DamageText;
	bool isFinished;
	public float PhysicalDamageTakeMultiplier = 1f;
	public float PhysicalDamageDealMultiplier = 1f;
	public float SoulDamageTakeMultiplier = 1f;
	public float SoulDamageDealMultiplier = 1f;
	//public GameObject Unit;
	public GameObject DamagePrefab;
	public GameObject SpPrefab;
	public GameObject HpPrefab;
	public GameObject DropPrefab;
	public GameObject BasePosition;
	//public Canvas DamageCanvas;
	public List<Tag> tagList = new List<Tag>();
	void Awake()
	{
		//DamageText.transform.
		if (jobData != null)
		{
			maxHP = jobData.JobStatsList[jobData.JobLevel - 1].maxHP;
			maxSP = jobData.JobStatsList[jobData.JobLevel - 1].maxSP;
			currentHP = jobData.currentHP;
			currentSP = jobData.currentSP;
			pa = jobData.JobStatsList[jobData.JobLevel - 1].physicalAttack;
			sa = jobData.JobStatsList[jobData.JobLevel - 1].soulAttack;
			pd = jobData.JobStatsList[jobData.JobLevel - 1].physicalDefence;
			sd = jobData.JobStatsList[jobData.JobLevel - 1].soulDefence;
			hit = jobData.JobStatsList[jobData.JobLevel - 1].hit;
			nim = jobData.JobStatsList[jobData.JobLevel - 1].nimbleness;
			cri = jobData.JobStatsList[jobData.JobLevel - 1].critical;
			Speed = jobData.JobStatsList[jobData.JobLevel - 1].speed;
			melee = jobData.JobStatsList[jobData.JobLevel - 1].melee;
			remote = jobData.JobStatsList[jobData.JobLevel - 1].remote;
			physicalDizziness = jobData.JobStatsList[jobData.JobLevel - 1].PhysicalDizziness;
			soulDizziness = jobData.JobStatsList[jobData.JobLevel - 1].SoulDizziness;
			expPerlevel = jobData.JobStatsList[jobData.JobLevel - 1].expPerLevel;
			sxpPerlevel = jobData.JobStatsList[jobData.JobLevel - 1].sxpPerLevel;
		}
		else
		{
			currentHP = EnemyData.currentHP;
			maxHP = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].maxHP;
			pa = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].physicalAtk;
			sa = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].soulAtk;
			pd = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].physicalDfs;
			sd = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].soulDfs;
			nim = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].nim;
			hit = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].hit;
			Speed = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].speed;
			AntiCri = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].AntiCri;
			pAntiDizziness = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].PhysicalAntiDizziness;
			sAntiDizziness = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].SoulAntiDizziness;
			dizzinessBar = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].dizzinessBar;
			exp = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].exp;
			sxp = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].sxp;
			melee = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].melee;
			remote = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].remote;
		}
		if (currentHP <= 0)
		{
			isDead = true;
		}

	}
	void FixedUpdate()
	{
		if (BattleSetting.Instance.CurrentActUnit != this.gameObject && BattleSetting.Instance.CurrentActUnitTarget != this.gameObject)
		{
			this.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
			//Debug.Log("变没了2");
		}
	}
	public void takeDamage(int Damage, AttackType attackType, bool isSelf)
	{
		currentHP -= Damage;
		if (isSelf)
		{
			StartCoroutine(OnSelfDamageTake());
		}
		else
		{
			StartCoroutine(OnDamageTake());
		}
		StartCoroutine(FloatingNumber(Damage, attackType));
	}

	public void takeBonusDamage(int Damage, AttackType attackType)
	{
		currentHP -= Damage;
		StartCoroutine(DroppingBonusNumber(Damage, attackType));
	}

	public IEnumerator FloatingMiss()
	{
		GameObject obj = Instantiate(DamagePrefab, BasePosition.transform);
		obj.GetComponent<Text>().text = "Miss";
		yield return new WaitForSeconds(2f);
		Destroy(obj);
	}

	IEnumerator FloatingNumber(int Damage, AttackType attackType)
	{
		GameObject obj = Instantiate(DamagePrefab, BasePosition.transform);
		obj.GetComponent<Text>().text = "-" + Damage;
		if (attackType == AttackType.Soul)
		{
			obj.GetComponent<Text>().color = new Color(0, 1, 1, 1);
		}
		if (currentHP <= 0)
		{
			isDead = true;
			currentHP = 0;
			if (this.gameObject.tag == "EnemyUnit")
			{
				this.gameObject.tag = "Untagged";
			}
		}
		yield return new WaitForSeconds(2f);
		Destroy(obj);
		if (currentHP <= 0)
		{
			isDead = true;
			currentHP = 0;
			if (BattleSetting.Instance.CurrentActUnit == gameObject)
			{
				BattleSetting.Instance.CurrentActUnit = null;
			}
			if (this.gameObject.tag == "PlayerUnit")
			{
				this.gameObject.tag = "Dead";
				this.gameObject.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(255, 255, 255, 0.5f);
			}
			else if (this.gameObject.tag == "Untagged")
			{
				this.gameObject.SetActive(false);
			}
		}
	}

	IEnumerator DroppingBonusNumber(int Damage, AttackType attackType)
	{
		GameObject obj = Instantiate(DropPrefab, BasePosition.transform);
		obj.GetComponent<Text>().text = "-" + Damage;
		if (attackType == AttackType.Soul)
		{
			obj.GetComponent<Text>().color = new Color(0, 1, 1, 1);
		}
		yield return new WaitForSeconds(0.45f);
		Destroy(obj);
		if (currentHP <= 0)
		{
			isDead = true;

			if (this.gameObject.tag == "PlayerUnit")
			{
				this.gameObject.tag = "Dead";
				this.gameObject.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(255, 255, 255, 0.5f);
			}
			else if (this.gameObject.tag == "EnemyUnit")
			{
				this.gameObject.SetActive(false);
			}
		}
	}

	public IEnumerator FloatingSP(int deltaTemp)
	{
		int temp;
		temp = currentSP;
		//Debug.Log(1);
		if (temp + deltaTemp >= maxSP)
		{
			deltaTemp = maxSP - currentSP;
			currentSP = maxSP;
		}
		else
		{
			currentSP += deltaTemp;
			if (currentSP < 0)
			{
				currentSP = 0;
			}
		}

		if (deltaTemp != 0)
		{
			GameObject obj = Instantiate(SpPrefab, BasePosition.transform);
			if (deltaTemp > 0)
			{
				obj.GetComponent<Text>().text = "+" + deltaTemp;
			}
			else
			{
				obj.GetComponent<Text>().text = "-" + deltaTemp;
			}
			yield return new WaitForSeconds(2f);
			Destroy(obj);
		}
	}

	public IEnumerator FloatingHP(int deltaTemp)
	{
		int temp;
		temp = currentHP;

		if (temp + deltaTemp >= maxHP)
		{
			deltaTemp = maxHP - currentHP;
			currentHP = maxHP;
		}
		else
		{
			currentHP += deltaTemp;
		}

		if (deltaTemp != 0)
		{
			GameObject obj = Instantiate(HpPrefab, BasePosition.transform);
			obj.GetComponent<Text>().text = "+" + deltaTemp;
			yield return new WaitForSeconds(2f);
			Destroy(obj);
		}
	}

	public void AddTagToCharacter(Tag tag)
	{
		if (tag.TagName == "Remote")
		{
			Tag newTag0 = Instantiate(tag);
			newTag0.Multiplier = remote / 100f;
			tagList.Add(newTag0);
			BattleSetting.Instance.CheckTagList(this.gameObject);
			return;
		}
		if (tag.TagName == "Melee")
		{
			Tag newTag0 = Instantiate(tag);
			newTag0.Multiplier = melee / 100f;
			tagList.Add(newTag0);
			BattleSetting.Instance.CheckTagList(this.gameObject);
			return;
		}
		if (tag is Bleed)
		{
			if (tagList.Exists(tag => tag is Bleed))
			{
				bool isAdded = false;
				foreach (Bleed bleed in tagList.FindAll(tag => tag is Bleed))
				{
					if (bleed.isSelf == ((Bleed)tag).isSelf)
					{
						bleed.TurnLast += tag.TurnAdd;
						isAdded = true;
					}
				}
				if (!isAdded)
				{
					Tag newTag0 = Instantiate(tag);
					tagList.Add(newTag0);
					BattleSetting.Instance.CheckTagList(this.gameObject);
					StartCoroutine(OnTagAdded(newTag0));
				}
			}
			else
			{
				Tag newTag0 = Instantiate(tag);
				tagList.Add(newTag0);
				BattleSetting.Instance.CheckTagList(this.gameObject);
				StartCoroutine(OnTagAdded(newTag0));
			}
			return;
		}
		if (tag is SpeedDown || tag is SpeedUp || tag is Poison)
		{
			Tag newTag0 = Instantiate(tag);
			tagList.Add(newTag0);
			BattleSetting.Instance.CheckTagList(this.gameObject);
			StartCoroutine(OnTagAdded(newTag0));
			return;
		}
		foreach (Tag existingTag in tagList)
		{
			if (existingTag.GetType() == tag.GetType())
			{
				switch (existingTag.TagKind)
				{
					case Tag.Kind.turnLessen:
						existingTag.TurnLast += existingTag.TurnAdd;
						break;
					case Tag.Kind.accumulable:
						existingTag.quantity += existingTag.quantity;
						break;
					default:
						break;
				}
				return;
			}
		}

		Tag newTag = Instantiate(tag);
		tagList.Add(newTag);
		BattleSetting.Instance.CheckTagList(this.gameObject);
		StartCoroutine(OnTagAdded(newTag));
	}

	public void CoroutineStart(IEnumerator enumerator)
	{
		StartCoroutine(enumerator);
	}

	/// <summary>
	/// 检测技能sp是否足够
	/// </summary>
	public void CheckSP()
	{

	}

	IEnumerator OnDamageTake()
	{
		foreach (var tag in tagList)
		{
			UnityAction OnDamageTake;
			OnDamageTake = tag.OnDamageTake;
			if (OnDamageTake == null)
			{
				continue;
			}
			OnDamageTake.Invoke();

			yield return null;
		}
	}

	IEnumerator OnSelfDamageTake()
	{
		foreach (var tag in tagList)
		{
			UnityAction OnSelfDamageTake;
			OnSelfDamageTake = tag.OnSelfDamageTake;
			if (OnSelfDamageTake == null)
			{
				continue;
			}
			OnSelfDamageTake.Invoke();

			yield return null;
		}
	}
	IEnumerator OnTagAdded(Tag tag)
	{
		UnityAction OnTagAdded;
		OnTagAdded = tag.OnTagAdded;
		if (OnTagAdded == null)
		{
			yield return null;
		}
		else
		{
			OnTagAdded.Invoke();
		}
	}
}
