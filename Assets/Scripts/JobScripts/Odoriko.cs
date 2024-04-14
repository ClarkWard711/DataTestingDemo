using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Odoriko : MonoBehaviour
{
    public class OdorikoBuff : Buff
    {
        public enum SkillKind {Moon, Sun};
        public SkillKind skillKind;
        public OdorikoBuff(Buff buff,SkillKind skillkind) : base(buff)
        {
            skillKind = skillkind;
            isTriggered = buff.isTriggered;
            BuffKind = buff.BuffKind;
            Impact = buff.Impact;
            BuffTarget = buff.BuffTarget;
            Effect = buff.Effect;
            TurnLast = buff.TurnLast;
            quantity = buff.quantity;
            Multiplier = buff.Multiplier;
        }

        public OdorikoBuff(string Name,bool istriggered, impactOnMultiplier impact, Kind buffkind, target bufftarget, effect effect, int turnLast, int Quantity, float multiplier,SkillKind skillkind) : base(Name,istriggered, impact, buffkind, bufftarget, effect, turnLast, Quantity, multiplier)
        {
            BuffName = Name;
            skillKind = skillkind;
            isTriggered = istriggered;
            Impact = impact;
            BuffKind = buffkind;
            BuffTarget = bufftarget;
            Effect = effect;
            TurnLast = turnLast;
            quantity = Quantity;
            Multiplier = multiplier;
        }
        //public static OdorikoBuff Moonlight = new OdorikoBuff(Buff.Defencing, OdorikoBuff.SkillKind.Moon);
    }
    
    //public Buff buff = new Buff(OdorikoBuff.Moonlight);
    public Button[] BasicSkillButton;
    public Button[] AdvancedSkillButton;
    bool MoonSpReduce = false, SunSpReduce = false;
    public float SpCostMultiplier = 1f;
    void Awake()
    {
        BasicSkillButton = BattleSetting.Instance.BasicPanel.GetComponentsInChildren<Button>();
        AdvancedSkillButton = BattleSetting.Instance.AdvancedPanel.GetComponentsInChildren<Button>();
        //SkillButton[0].onClick.AddListener(() => Moonlight(BattleSetting.Instance.CurrentActUnit));
        //buff.skillKind = OdorikoBuff.SkillKind.Moon;
        //Buff Test = new Buff(buff);
    }

    void Moonlight(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        MoonSpCost();
        Buff defencing = new Buff(BattleSetting.Instance.Defencing);
        unit.GetComponent<GivingData>().BuffList.Add(defencing);
        BattleSetting.Instance.CheckBuffList(unit);
        defencing.TurnLast++;
        if (unit.GetComponent<GivingData>().BuffList.Exists(Buff => Buff.BuffName == "蓄力")) 
        {
            MoonSpReduce = true;
            SpCostMultiplier = 0.8f;
        }
        else
        {
            MoonSpReduce = true;
            SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.State = BattleState.Middle;
        StartCoroutine(BattleSetting.Instance.ShowActionText("月光"));
    }

    void SunSpot(GameObject unit)
    {
        if (BattleSetting.Instance.State != BattleState.PlayerTurn) return;
        
        BattleSetting.Instance.isWaitForPlayerToChooseUnit = true;
        StartCoroutine(SunSpot());
    }
    IEnumerator SunSpot()
    {
        yield return new WaitUntil(() => BattleSetting.Instance.isChooseFinished);
        SunSpCost();
        if (BattleSetting.Instance.CurrentActUnit.GetComponent<GivingData>().BuffList.Exists(Buff => Buff.BuffName == "蓄力"))
        {
            SunSpReduce = true;
            SpCostMultiplier = 0.8f;
        }
        else
        {
            SunSpReduce = true;
            SpCostMultiplier = 0.9f;
        }
        BattleSetting.Instance.isChooseFinished = false;
        BattleSetting.Instance.GameStateText.text = "日斑";
        StartCoroutine(BattleSetting.Instance.ShowText(1f));
        StartCoroutine(BattleSetting.Instance.DealDamage(3f));
    }

    void MoonSpCost()
    {
        if (MoonSpReduce)
        {
            MoonSpReduce = false;
        }
        else
        {
            SpCostMultiplier = 1f;
        }
    }

    void SunSpCost()
    {
        if (SunSpReduce)
        {
            SunSpReduce = false;
        }
        else
        {
            SpCostMultiplier = 1f;
        }
    }
}
