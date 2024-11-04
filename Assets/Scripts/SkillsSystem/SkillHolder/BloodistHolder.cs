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
        BattleSetting.Instance.DelimaPanel.SetActive(false);
        if (optionIndex == 1)
        {
            //伤害
        }
        else if (optionIndex == 2) 
        {
            //回复sp
        }
        StartCoroutine(BattleSetting.Instance.ShowActionText("血散"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();
    }

    public void Choose(int optionNum)
    {
        optionIndex = optionNum;
        isChooseFin = true;
    }
}
