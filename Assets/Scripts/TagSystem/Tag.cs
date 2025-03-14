using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Tag : ScriptableObject
{
    //基础信息
    public int ID;
    public string TagName;//Tag名称
    public bool isTriggered;//是否触发
    public enum Kind { accumulable, turnLessen, eternal, trigger, special };//累积类，随回合减少，永久
    public enum target { ally, enemy, all, self };//tag给到谁身上
    public enum impactOnMultiplier { PhysicalTake, PhysicalDeal, SoulTake, SoulDeal, AllDeal, AllTake };//tag对倍率的影响
    public enum effect { good, bad, neutral };//tag效果
    public impactOnMultiplier Impact;
    public Kind TagKind;
    public target BuffTarget;
    public effect Effect;
    public int TurnAdd;//增加多少回合
    public int TurnLast;//还有多少个回合
    public int quantity;//积累多少个
    public float Multiplier = 1f;//倍率
    public float conditionMultiplier = 1f;
    public int hit = 0, nim = 0, spd = 0, cri = 0, antiCri = 0;
    //回调点
    public virtual void OnTurnStartCallback()
    {

    }

    public virtual void OnTurnEndCallback()
    {

    }

    public virtual void OnActionEndCallback()
    {

    }
    //命中前后回调点
    public UnityAction OnHit, BeforeHit, BeforeBeingHit, BeingHit, OnSelfDamageTake, OnDamageTake, OnDealDamage, OnTagAdded;
}
