using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class GivingData : MonoBehaviour
{
    public JobData jobData;
    public EnemyData EnemyData;
    public int Speed,currentHP,currentSP,pa,sa,pd,sd,hit,nim,cri,melee,remote,maxHP,maxSP,miss,AntiCri;
    public bool isDead = false;
    //public Text DamageText;
    bool isFinished;
    public float PhysicalDamageTakeMultiplier = 1f;
    public float PhysicalDamageDealMultiplier = 1f;
    public float SoulDamageTakeMultiplier = 1f;
    public float SoulDamageDealMultiplier = 1f;
    //public GameObject Unit;
    public GameObject DamagePrefab;
    public GameObject BasePosition;
    //public Canvas DamageCanvas;
    public List<Tag> tagList = new List<Tag>();
    void Awake()
    {
        //DamageText.transform.
        if (jobData!=null)
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
            Speed = jobData.JobStatsList[jobData.JobLevel-1].speed;
            melee = jobData.JobStatsList[jobData.JobLevel - 1].melee;
            remote = jobData.JobStatsList[jobData.JobLevel - 1].remote;

        }
        else
        {
            currentHP = EnemyData.currentHP;
            maxHP = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].maxHP;
            pa = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].physicalAtk;
            sa = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].soulAtk;
            pd = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].physicalDfs;
            sd = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].soulDfs;
            miss = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].miss;
            hit = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].hit;
            Speed = EnemyData.EnemyStatsList[EnemyData.EnemyLevel-1].speed;
            AntiCri = EnemyData.EnemyStatsList[EnemyData.EnemyLevel - 1].AntiCri;
            melee = 100;
            remote = 100;
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
    public void takeDamage(int Damage)
    {
        currentHP -= Damage;
        StartCoroutine(FloatingNumber(Damage));
    }

    IEnumerator FloatingNumber(int Damage)
    {
        GameObject obj = Instantiate(DamagePrefab, BasePosition.transform);
        obj.GetComponent<Text>().text = "-" + Damage;
        yield return new WaitForSeconds(2f);
        Destroy(obj);
        if (currentHP <= 0)
        {
            isDead = true;

            if (this.gameObject.tag == "PlayerUnit")
            {
                this.gameObject.tag = "Untagged";
                this.gameObject.GetComponentsInChildren<SpriteRenderer>()[0].color = new Color(255, 255, 255, 0.5f);
            }
            else if (this.gameObject.tag == "EnemyUnit")
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void AddTagToCharacter(Tag tag)
    {
        if (tag.name == "Remote") 
        {
            Tag newTag0 = Instantiate(tag);
            newTag0.Multiplier = remote / 100;
            tagList.Add(newTag0);
            BattleSetting.Instance.CheckTagList(this.gameObject);
            return;
        }
        if (tag.name == "Melee")
        {
            Tag newTag0 = Instantiate(tag);
            newTag0.Multiplier = melee / 100;
            tagList.Add(newTag0);
            BattleSetting.Instance.CheckTagList(this.gameObject);
            return;
        }
        foreach (Tag existingTag in tagList)
        {
            if (existingTag.GetType() == tag.GetType())
            {
                existingTag.TurnLast += existingTag.TurnAdd;
                return;
            }
        }

        Tag newTag = Instantiate(tag);
        tagList.Add(newTag);
        BattleSetting.Instance.CheckTagList(this.gameObject);
    }
}
