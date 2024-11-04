using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;




public class CelestialSeerHolder : JobSkillHolder
{

    public bool isChoseFin = false;
    public int state = 0;
    public static CelestialSeerHolder Instance;


    public override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Start()
    {
        gameObject.GetComponent<GivingData>().AddTagToCharacter(JadeFall.CreateInstance<JadeFall>());
    }

    public IEnumerator rambleWithSky(int SpCost, CsSkillKind csSkillKind)
    {
        BattleSetting.Instance.canChangeAction = false;
        BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().currentSP -= SpCost;
        BattleSetting.Instance.DelimaPanel.SetActive(true);
        var choice1 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
        var choice2 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
        var choice3 = Instantiate(BattleSetting.Instance.DelimaActionPrefab, BattleSetting.Instance.DelimaPanel.transform);
        choice1.GetComponentInChildren<Text>().text = "日之辉耀";
        choice2.GetComponentInChildren<Text>().text = "月之皎洁";
        choice3.GetComponentInChildren<Text>().text = "星之鼓动";

        //todo:加条件检测，如果处于这个状态就不能进入这个状态
        if (gameObject.GetComponent<GivingData>().tagList.Exists(tag => tag is GlaringlySun))
        {
            choice1.GetComponent<Button>().enabled = false;
        } else if (true) {

        } else if (true)
        {

        }


        choice1.GetComponent<Button>().onClick.AddListener(() => ChooseThree(1));
        choice2.GetComponent<Button>().onClick.AddListener(() => ChooseThree(2));
        choice3.GetComponent<Button>().onClick.AddListener(() => ChooseThree(3));

        yield return new WaitUntil(() => isChoseFin);
        BattleSetting.Instance.DelimaPanel.SetActive(false);
        Destroy(choice1);
        Destroy(choice2);
        Destroy(choice3);
        foreach (var player in BattleSetting.Instance.RemainingPlayerUnits)
        {
            player.GetComponent<GivingData>().tagList.RemoveAll(tag => tag is CsState);
        }
        switch (state)
        {
            case 0:
                break;
            case 1:
                gameObject.GetComponent<GivingData>().AddTagToCharacter(GlaringlySun.CreateInstance<GlaringlySun>());
                break;
            //todo :case 2/3:

        }

        //CsState test1 = (CsState)gameObject.GetComponent<GivingData>().tagList.Find(tag => tag is CsState);
        //test1.isEnhanced = false;
        StartCoroutine(BattleSetting.Instance.ShowActionText("碧天伴走"));
        yield return new WaitForSeconds(1f);
        BattleSetting.Instance.ActionEnd();

    }

    public void ChooseThree(int stateNumber)
    {
        state = stateNumber;
        isChoseFin = true;
    }




}
