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
    public float DamageTakeMultiplier = 1f;
    public float DamageDealMultiplier = 1f;
    //public GameObject Unit;
    public GameObject DamagePrefab;
    public GameObject BasePosition;
    //public Canvas DamageCanvas;
    void Awake()
    {
        //DamageText.transform.
        if (jobData!=null)
        {
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
        if (currentHP <= 0)
        {
            isDead = true;
        }
    }

    IEnumerator FloatingNumber(int Damage)
    {
        GameObject obj = Instantiate(DamagePrefab, BasePosition.transform);
        obj.GetComponent<Text>().text = "-" + Damage;
        yield return new WaitForSeconds(2f);
        Destroy(obj);
    }
}
