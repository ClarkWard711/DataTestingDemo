using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Data;
using UnityEngine.Events;


public enum BattleState {Won,Lose,PlayerTurn,EnemyTurn,Start,Middle};

public class BattleSetting : MonoBehaviour
{
    #region public
    public static BattleSetting Instance;
    GameObject Player;
    [SerializeField] AssetReference outerScene;
    //[SerializeField] List<int> SpdList;
    //测试变了没
    public List<GameObject> BattleUnitsList;//战斗单位
    public List<GameObject> BattleUnitsListToBeLaunched;//第二战斗单位
    public GameObject[] playerUnits;//玩家单位
    public GameObject[] enemyUnits;//敌方单位
    public GameObject[] RemainingEnemyUnits;//剩余敌人单位
    public GameObject[] RemainingPlayerUnits;//剩余玩家单位
    public GameObject CurrentActUnit;//当前行动角色
    public GameObject CurrentActUnitTarget;//当前行动角色的目标
    public GameObject CurrentEndTurnUnit;//当前执行回合结束角色
    public GameObject MovePanel;//移动选择的panel
    GameObject ShownUnit;//ui展示的单位
    GameObject CurrentSliderOwner;//ui显示单位的slider
    public List<GameObject> PlayerPositionsList;//我方位置
    public List<GameObject> EnemyPositionsList;//敌方位置

    public GameObject SkillList;//基础进阶技能显示
    bool isTextShowed = false;//基础进阶是否显示
    public GameObject BasicPanel, AdvancedPanel;//基础技能和进阶技能的panel
    public bool isBasicShowed = false;//基础技能是否显示
    public bool isAdvancedShowed = false;//进阶技能是否显示
    List<int> SkillID = new List<int>();//技能ID

    public Text GameStateText;//对战状态文本
    //public Button AtkButton;
    public Image Avatar;//ui显示头像
    Ray TargetChosenRay;//选择激光
    RaycastHit2D TargetHit;//选择激光目标
    //RaycastHit2D[] TargetHitResult;
    
    public Slider HpSlider;//血条
    public Slider SpSlider;//蓝条

    public PartyMember PlayerPartyMember;//我方队伍角色
    public EnemyParty EnemyPartyMember;//敌方队伍角色

    //public bool isWaitForPlayerToChooseAction = false;//等待玩家选择操作
    public bool isWaitForPlayerToChooseUnit = false;//等待玩家选择单位
    public bool isWaitForPlayerToChooseAlly = false;//等待玩家选择友方
    public bool isPressed = false;//回主世界是否按下
    bool isMoving = false;//鼠标有没有在移动
    bool isKeyboardTouched = false;//键盘有没有碰 用于键盘操控ui
    public bool isMoveFinished = false;//移动完了没 用于前后排tag
    public BattleState State = BattleState.Start;//战斗进度状态


    public bool isChooseFinished = false;//玩家选完了没
    public bool isCri;
    public bool isActionEnding = false;
    public bool isTurnEnding = false;
    public bool canChangeAction = false;
    bool isNormalAttack = false;

    float alpha;//颜色透明度
    //float DamageMultiplier = 1f;
    public Vector3 Position;
    #endregion

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (GameObject.Find("Player") != null)
        {
            Player = GameObject.Find("Player");
            Player.GetComponentInChildren<Camera>().enabled = false;
            GameObject.Find("Player").SetActive(false);
        }

        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>().enabled = true;
        BattleUnitsList = new List<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            if (PlayerPartyMember.CharacterList[i] != null)
            {
                Instantiate(PlayerPartyMember.CharacterList[i].JobPrefab, PlayerPositionsList[i].transform);
            }
            if (EnemyPartyMember.EnemyDataList[i] != null)
            {
                Instantiate(EnemyPartyMember.EnemyDataList[i].JobPrefab, EnemyPositionsList[i].transform);
            }
        }

        playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        foreach (GameObject PlayerUnit in playerUnits)
        {
            BattleUnitsList.Add(PlayerUnit);
        }

        enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        foreach (GameObject EnemyUnit in enemyUnits)
        {
            BattleUnitsList.Add(EnemyUnit);
        }
        CheckPositionID();
        ComparePosition();
        BattleUnitsList.Sort((x, y) => -x.GetComponent<GivingData>().Speed.CompareTo(y.GetComponent<GivingData>().Speed));
        State = BattleState.Start;
        StartCoroutine(TurnAction(2f, "对战开始"));
    }

    private void Update()
    {
        if (Input.mousePosition != Position)
        {
            isMoving = true;
            isKeyboardTouched = false;
        }
        else if (Input.mousePosition == Position && isKeyboardTouched) 
        {
            isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        #region 返回主世界
        if (Input.GetKeyDown(KeyCode.E) && !isPressed)
        {
            isPressed = true;
            if (Player != null) 
            {
                Player.SetActive(true);
            }
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>().enabled = false;
            SceneLoader.LoadAddressableScene(outerScene);
        }
        if ((State == BattleState.Won || State == BattleState.Lose) && !isPressed) 
        {
            isPressed = true;
            if (Player != null)
            {
                Player.SetActive(true);
            }
            SceneLoader.LoadAddressableScene(outerScene);
        }
        #endregion

        Position = Input.mousePosition;
        ChoosingEnemy();
        ChoosingAlly();
        if (CurrentSliderOwner != null) 
        {
            UpdateSliderChange();
        }
    }

    /*void ListSort()
    {
        BattleUnitsList.Sort((x, y) => x.GetComponent<GivingData>().Speed.CompareTo(y.GetComponent<GivingData>().Speed));
        var temp = BattleUnitsList[0];
        //var temp = enemyUnits[1].GetComponent<Enemy>().EnemyData.EnemyStatsList[enemyUnits[1].GetComponent<Enemy>().EnemyData.EnemyLevel].speed;
        for (int i = 0; i < BattleUnitsList.Count - 1; i++)
        {
            int MaxValue = BattleUnitsList[i].GetComponent<GivingData>().Speed;
            int MaxIndex = i;

            for (int j = i + 1; j < BattleUnitsList.Count; j++)
            {
                if (MaxValue < BattleUnitsList[j].GetComponent<GivingData>().Speed)
                {
                    MaxValue = BattleUnitsList[j].GetComponent<GivingData>().Speed;
                    MaxIndex = j;
                }
            }
            temp = BattleUnitsList[i];
            BattleUnitsList[i] = BattleUnitsList[MaxIndex];
            BattleUnitsList[MaxIndex] = temp;
        }
    }*/

    public void ToBattle()
    {
        //Debug.Log(1);
        RemainingEnemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        RemainingPlayerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        State = BattleState.Middle;

        if (BattleUnitsList.Count == 0) 
        {
            StartCoroutine(EndTurn());
            return;
        }

        if (RemainingEnemyUnits.Length == 0)
        {
            GameStateText.text = "胜利";
            StartCoroutine(ShowText(3f));
            State = BattleState.Won;
        }
        else if (RemainingPlayerUnits.Length == 0)
        {
            GameStateText.text = "失败";
            StartCoroutine(ShowText(3f));
            State = BattleState.Lose;
        }
        else
        {
            CurrentActUnit = BattleUnitsList[0];
            BattleUnitsList.Remove(CurrentActUnit);
            //BattleUnitsList.Add(CurrentActUnit);
            BattleUnitsListToBeLaunched.Add(CurrentActUnit);

            if (!CurrentActUnit.GetComponent<GivingData>().isDead)
            {
                FindTarget();
            }
            else
            {
                ToBattle();
            }
        }

    }

    #region 协程
    public IEnumerator ShowText(float time)
    {
        SetColorTo1(GameStateText);
        yield return new WaitForSeconds(time);
        SetColorTo0(GameStateText);
    }

    public IEnumerator DealDamage(float time)
    {
        State = BattleState.Middle;
        AttackType ActAttakeType;
        ActAttakeType = CurrentActUnit.GetComponent<GivingData>().attackType;
        int Damage = DamageCounting(ActAttakeType);
        if (CurrentActUnit.GetComponent<GivingData>().tagList.Exists(Tag => Tag.TagName == "Charging") && isNormalAttack) 
        {
            Damage = Mathf.CeilToInt(2f * Damage);
        }
        //加入分配伤害检测
        /*if (CheckHit())
        {
            isCri = CheckCri();
            //命中检测成功回调
            if (isCri)
            {
                TempDamage = Mathf.CeilToInt(Damage * 1.5f);
                Damage = TempDamage;
                StartCoroutine(BeforeHit());
                Damage = TempDamage;
                CurrentActUnitTarget.GetComponent<GivingData>().takeDamage(Damage);
                GameStateText.text = "对" + CurrentActUnitTarget.name + "造成暴击伤害" + Damage;
                StartCoroutine(ShowText(2f));
            }
            else
            {
                TempDamage = Damage;
                StartCoroutine(BeforeHit());
                Damage = TempDamage;
                CurrentActUnitTarget.GetComponent<GivingData>().takeDamage(Damage);
                GameStateText.text = "对" + CurrentActUnitTarget.name + "造成伤害" + Damage;
                StartCoroutine(ShowText(2f));
            }
        }
        else
        {
            GameStateText.text = "Miss";
            StartCoroutine(ShowText(2f));
        }*/
        StartCoroutine(BeforeHit());
        CurrentActUnitTarget.GetComponent<GivingData>().takeDamage(Damage, ActAttakeType);
        GameStateText.text = "对" + CurrentActUnitTarget.name + "造成伤害" + Damage;
        StartCoroutine(ShowText(2f));
        //受击回调
        StartCoroutine(OnHit());
        StartCoroutine(BeingHit());
        
        yield return new WaitForSeconds(time);
        CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Null;
        isNormalAttack = false;
        ActionEnd();
    }

    public IEnumerator DealDamageBonus(int Damage,AttackType attackType)
    {
        State = BattleState.Middle;
        yield return new WaitForSeconds(0.5f);
        CurrentActUnitTarget.GetComponent<GivingData>().takeDamage(Damage, attackType);
        GameStateText.text = "对" + CurrentActUnitTarget.name + "造成追击伤害" + Damage;
        StartCoroutine(ShowText(1f));
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator DealCounterDamage(int Damage,AttackType attackType)
    {
        State = BattleState.Middle;
        CurrentActUnit.GetComponent<GivingData>().takeDamage(Damage, attackType);
        GameStateText.text = "对" + CurrentActUnit.name + "造成反击伤害" + Damage;
        StartCoroutine(ShowText(1f));
        yield return new WaitForSeconds(1f);
    }

    IEnumerator TurnAction(float time, string text)
    {
        GameStateText.text = text;
        StartCoroutine(ShowText(1f));
        yield return new WaitForSeconds(time);

        if (State == BattleState.EnemyTurn)
        {
            int TargetIndex = Random.Range(0, RemainingPlayerUnits.Length);
            CurrentActUnitTarget = RemainingPlayerUnits[TargetIndex];
            CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Physical;
            StartCoroutine(DealDamage(3f));
        }
        else if (State == BattleState.PlayerTurn)
        {
            GameStateText.text = "选择操作";
            StartCoroutine(ShowText(2f));
        }
        else if (State == BattleState.Start)
        {
            ToBattle();
        }
    }

    public IEnumerator ShowActionText(string Action)
    {
        GameStateText.text = Action;
        StartCoroutine(ShowText(1f));
        yield return new WaitForSeconds(1f);
    }

    IEnumerator Charge()
    {
        StartCoroutine(ShowActionText("蓄力"));
        yield return new WaitForSeconds(1.5f);
        ActionEnd();
    }

    IEnumerator Move()
    {
        MovePanel.SetActive(true);
        yield return new WaitUntil(() =>isMoveFinished);
        canChangeAction = false;

        MovePanel.SetActive(false);

        if (State != BattleState.PlayerTurn) 
        {
            GameStateText.text = "移动";
            StartCoroutine(ShowText(2f));
            ComparePosition();
            yield return new WaitForSeconds(2f);
            ActionEnd();
        }
        
    }

    IEnumerator Attack()
    {
        yield return new WaitUntil(() => isChooseFinished);
        canChangeAction = false;
        isChooseFinished = false;
        isNormalAttack = true;
        CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Physical;
        StartCoroutine(DealDamage(3f));
    }
    /// <summary>
    /// 命中检测成功后的回调
    /// </summary>
    /// <returns></returns>
    IEnumerator BeforeHit()
    {
        foreach (Tag tag in CurrentActUnit.GetComponent<GivingData>().tagList)
        {
            UnityAction BeforeHit;
            BeforeHit = tag.BeforeHit;
            if (BeforeHit == null)
            {
                continue;
            }
            BeforeHit.Invoke();

            yield return null;
        }
    }
    /// <summary>
    /// 在命中之后回调的操作
    /// </summary>
    /// <returns></returns>
    IEnumerator OnHit()
    {
        foreach (Tag tag in CurrentActUnit.GetComponent<GivingData>().tagList)
        {
            UnityAction OnHit;
            OnHit = tag.OnHit;
            if (OnHit == null) 
            {
                continue;
            }
            OnHit.Invoke();

            yield return StartCoroutine(DelayedCallback(1f));
        }
    }
    /// <summary>
    /// 被攻击方被命中后的回调
    /// </summary>
    /// <returns></returns>
    IEnumerator BeingHit()
    {
        foreach (Tag tag in CurrentActUnitTarget.GetComponent<GivingData>().tagList)
        {
            UnityAction BeingHit;
            BeingHit = tag.BeingHit;
            if (BeingHit == null)
            {
                continue;
            }
            BeingHit.Invoke();

            yield return StartCoroutine(DelayedCallback(1f));
        }
    }
    /// <summary>
    /// 一整个回合结束之后的tag进行操作
    /// </summary>
    /// <returns></returns>
    IEnumerator EndTurn()
    {
        isTurnEnding = true;
        BattleUnitsList.AddRange<GameObject>(BattleUnitsListToBeLaunched);
        BattleUnitsListToBeLaunched.Clear();
        foreach (GameObject character in BattleUnitsList)
        {
            CurrentEndTurnUnit = character;
            foreach (var tag in character.GetComponent<GivingData>().tagList)
            {
                var method = tag.GetType().GetMethod("OnTurnEndCallback");
                if (method.DeclaringType == typeof(Tag))
                {
                    // Tag does not override OnTurnEndCallback, so skip to the next tag
                    continue;
                }
                tag.OnTurnEndCallback();

                yield return StartCoroutine(DelayedCallback(2f));
            }
        }
        CurrentEndTurnUnit = null;
        TurnSettle();
    }
    /// <summary>
    /// 跟EndTurn和ActionEndCallBack绑定的在tag进行操作之后
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    public IEnumerator DelayedCallback(float delay)
    {
        Debug.Log("delaying");
        yield return new WaitForSeconds(delay);
        Debug.Log("delay Finished");
        // 在这里执行需要延迟的后续操作
    }
    /// <summary>
    /// 重复执行某一方法并产生延迟
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public IEnumerator MethodActivateDelay(UnityAction action,float delay,int time)
    {
        for (int i = 0; i < time; i++)
        {
            action?.Invoke();
            Debug.Log("delaying");
            yield return new WaitForSeconds(delay);
            Debug.Log("delay Finished");
            // 在这里执行需要延迟的后续操作
        }
    }
    #endregion

    #region SetColor
    //todo:设置不同颜色 

    void SetColorTo0(Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }


    void SetColorTo1(Text text)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        //alpha = 1;放在造成伤害之后
    }

    void SetColorTo0Gradually(Text text)
    {
        if (alpha > 0)
        {
            alpha -= 0.01f;
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
    #endregion

    void FindTarget()
    {
        if (CurrentActUnit.tag == "EnemyUnit")
        {
            State = BattleState.EnemyTurn;
            StartCoroutine(TurnAction(1f, "敌方回合"));
        }
        else
        {
            State = BattleState.PlayerTurn;
            CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 255);
            CurrentActUnit.GetComponent<GivingData>().CheckSP();
            UpdateUIPanel();
            StartCoroutine(TurnAction(1f, "你的回合"));
        }
    }

    #region 玩家按钮对应操作

    public void OnAtkButton()
    {
        CheckCanChangeAction();
        if (State != BattleState.PlayerTurn) return;
        canChangeAction = true;
        isWaitForPlayerToChooseUnit = true;
        StartCoroutine(Attack());
    }

    public void OnDefButton()
    {
        CheckCanChangeAction();
        if (State != BattleState.PlayerTurn) return;
        canChangeAction = false;
        CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(Defense.CreateInstance<Defense>());
        CheckTagList(CurrentActUnit);
        State = BattleState.Middle;
        StartCoroutine(ShowActionText("防御"));
    }

    public void OnMoveButton()
    {
        CheckCanChangeAction();
        if (State != BattleState.PlayerTurn) return;
        canChangeAction = true;
        isWaitForPlayerToChooseUnit = false;
        CurrentActUnitTarget = null;
        State = BattleState.Middle;
        isMoveFinished = false;
        StartCoroutine(Move());
        
    }

    public void OnChargeButton()
    {
        CheckCanChangeAction();
        if (State != BattleState.PlayerTurn) return;
        canChangeAction = false;
        CurrentActUnit.GetComponent<GivingData>().AddTagToCharacter(Charging.CreateInstance<Charging>());
        CheckTagList(CurrentActUnit);
        State = BattleState.Middle;
        StartCoroutine(Charge());
    }

    public void OnSKillButton()
    {
        if (!isTextShowed)
        {
            SkillList.SetActive(true);
            isTextShowed = true;
        }
        else
        {
            SkillList.SetActive(false);
            isTextShowed = false;
        }

        if (isBasicShowed)
        {
            BasicPanel.SetActive(false);
            isBasicShowed = false;
        }

        if (isAdvancedShowed)
        {
            AdvancedPanel.SetActive(false);
            isAdvancedShowed = false;
        }
    }
    
    public void OnBasicButton()
    {
        if (!isBasicShowed)
        {
            BasicPanel.SetActive(true);
            AdvancedPanel.SetActive(false);
            isBasicShowed = true;
            isAdvancedShowed = false;
        }
        else
        {
            BasicPanel.SetActive(false);
            isBasicShowed = false;
        }
    }

    public void OnAdvancedButton()
    {
        if (!isAdvancedShowed)
        {
            AdvancedPanel.SetActive(true);
            BasicPanel.SetActive(false);
            isAdvancedShowed = true;
            isBasicShowed = false;
        }
        else
        {
            AdvancedPanel.SetActive(false);
            isAdvancedShowed = false;
        }
    }
    #endregion

    /// <summary>
    /// 伤害计算
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="dfs"></param>
    /// <param name="TakeMultiplier"></param>
    /// <param name="DealMultiplier"></param>
    /// <returns></returns>
    public int DamageCounting(AttackType attackType)
    {
        int baseDamage;
        int finalDamage;
        int atk, dfs;
        float TakeMultiplier = 1f, DealMultiplier = 1f;
        //技能和tag对伤害的影响 还有分配 有待完善
        if (attackType == AttackType.Physical)
        {
            atk = CurrentActUnit.GetComponent<GivingData>().pa;
            dfs = CurrentActUnitTarget.GetComponent<GivingData>().pd;
            TakeMultiplier = CurrentActUnitTarget.GetComponent<GivingData>().PhysicalDamageTakeMultiplier;
            DealMultiplier = CurrentActUnit.GetComponent<GivingData>().PhysicalDamageDealMultiplier;
            baseDamage = Mathf.CeilToInt((atk * atk) / (atk + dfs));
            finalDamage = Mathf.CeilToInt(baseDamage * TakeMultiplier * DealMultiplier);
            return finalDamage;
        }
        else if(attackType == AttackType.Soul)
        {
            atk = CurrentActUnit.GetComponent<GivingData>().sa;
            dfs = CurrentActUnitTarget.GetComponent<GivingData>().sd;
            TakeMultiplier = CurrentActUnitTarget.GetComponent<GivingData>().SoulDamageTakeMultiplier;
            DealMultiplier = CurrentActUnit.GetComponent<GivingData>().SoulDamageDealMultiplier;
            baseDamage = Mathf.CeilToInt((atk * atk) / (atk + dfs));
            finalDamage = Mathf.CeilToInt(baseDamage * TakeMultiplier * DealMultiplier);
            return finalDamage;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 通过输入单位来计算伤害
    /// </summary>
    /// <param name="attackType"></param>
    /// <returns></returns>
    public int DamageCountingByUnit(GameObject AtkUnit, GameObject DfsUnit, AttackType attackType)
    {
        int baseDamage;
        int finalDamage;
        int atk, dfs;
        float TakeMultiplier = 1f, DealMultiplier = 1f;
        //技能和tag对伤害的影响 还有分配 有待完善
        if (attackType == AttackType.Physical)
        {
            atk = AtkUnit.GetComponent<GivingData>().pa;
            dfs = DfsUnit.GetComponent<GivingData>().pd;
            TakeMultiplier = DfsUnit.GetComponent<GivingData>().PhysicalDamageTakeMultiplier;
            DealMultiplier = AtkUnit.GetComponent<GivingData>().PhysicalDamageDealMultiplier;
            baseDamage = Mathf.CeilToInt((atk * atk) / (atk + dfs));
            finalDamage = Mathf.CeilToInt(baseDamage * TakeMultiplier * DealMultiplier);
            return finalDamage;
        }
        else if (attackType == AttackType.Soul)
        {
            atk = AtkUnit.GetComponent<GivingData>().sa;
            dfs = DfsUnit.GetComponent<GivingData>().sd;
            TakeMultiplier = DfsUnit.GetComponent<GivingData>().SoulDamageTakeMultiplier;
            DealMultiplier = AtkUnit.GetComponent<GivingData>().SoulDamageDealMultiplier;
            baseDamage = Mathf.CeilToInt((atk * atk) / (atk + dfs));
            finalDamage = Mathf.CeilToInt(baseDamage * TakeMultiplier * DealMultiplier);
            return finalDamage;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 命中概率计算
    /// </summary>
    /// <returns></returns>
    float HitChanceCounting()
    {
        float HitChance;
        HitChance = CurrentActUnit.GetComponent<GivingData>().hit / (CurrentActUnit.GetComponent<GivingData>().hit + CurrentActUnitTarget.GetComponent<GivingData>().nim);
        //加入技能或者其他tag对命中率的检测
        //CurrentActUnit.GetComponent<GivingData>().tagList
        return HitChance;
    }
    /// <summary>
    /// 暴击概率计算
    /// </summary>
    /// <returns></returns>
    float CriChanceCounting()
    {
        float CriChance;
        CriChance = CurrentActUnit.GetComponent<GivingData>().cri / CurrentActUnitTarget.GetComponent<GivingData>().AntiCri;
        return CriChance;
    }
    /// <summary>
    /// 鼠标选择敌方
    /// </summary>
    void ChoosingEnemy()
    {
        if (isWaitForPlayerToChooseUnit)
        {
            if (isMoving && !isKeyboardTouched)
            {
                if (CurrentActUnitTarget != null)
                {
                    CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                }
                isKeyboardTouched = false;
                TargetChosenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.Log(TargetChosenRay);
                TargetHit = Physics2D.Raycast(TargetChosenRay.origin, Vector2.down);
                if (ShownUnit != null)
                {
                    if (TargetHit.collider == null || TargetHit.collider.gameObject != ShownUnit)
                    {
                        //Debug.Log("移出collider");
                        //Debug.Log("变没了1");
                        ShownUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                    }
                }
                if (TargetHit.collider != null)
                {
                    //CurrentActUnitTarget = TargetHit.collider.gameObject;
                    //Debug.Log(TargetHit.collider.gameObject.name);
                    //Debug.Log(TargetChosenRay);
                    if (TargetHit.collider.gameObject.tag == "EnemyUnit")
                    {
                        CurrentActUnitTarget = TargetHit.collider.gameObject;
                        ShownUnit = TargetHit.collider.gameObject;
                        //Debug.Log("变了");
                        TargetHit.collider.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 255);
                    }

                    if (Input.GetMouseButtonDown(0) && TargetHit.collider.gameObject.tag == "EnemyUnit")
                    {
                        //CurrentActUnitTarget = TargetHit.collider.gameObject;
                        ShownUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        isWaitForPlayerToChooseUnit = false;
                        //StartCoroutine(DealDamage(3f));
                        isChooseFinished = true;
                        CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                    }

                    if (Input.GetKey(KeyCode.Return) && TargetHit.collider.gameObject.tag == "EnemyUnit")
                    {
                        CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        isWaitForPlayerToChooseUnit = false;
                        StartCoroutine(DealDamage(3f));
                        CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                    }
                }
            }
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                isKeyboardTouched = true;
                //增加如果target不存在的检测
                if (CurrentActUnitTarget == null)
                {
                    CurrentActUnitTarget = enemyUnits[0];
                }
                CurrentActUnitTarget.GetComponent<Collider2D>().enabled = false;
                //TargetChosenRay = new Ray(CurrentActUnitTarget.transform.position, );
                //TargetHit = Physics2D.Raycast(CurrentActUnitTarget.transform.position, Vector2.down);
                //Debug.Log(TargetHit.collider.gameObject);
                CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 255);

                if (Input.GetButton("Horizontal"))
                {
                    float direction = Input.GetAxisRaw("Horizontal");
                    TargetHit = Physics2D.Raycast(CurrentActUnitTarget.transform.position, new Vector2(direction, 0));
                    if (TargetHit.collider != null && TargetHit.collider.gameObject.tag == "EnemyUnit")
                    {
                        //Debug.Log(TargetHit.collider.gameObject);
                        CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                        CurrentActUnitTarget = TargetHit.collider.gameObject;
                    }
                    //Debug.Log(TargetHit.collider.gameObject);
                }

                if (Input.GetButton("Vertical"))
                {
                    float direction = Input.GetAxisRaw("Vertical");
                    TargetHit = Physics2D.Raycast(CurrentActUnitTarget.transform.position, new Vector2(0, direction));
                    if (TargetHit.collider != null && TargetHit.collider.gameObject.tag == "EnemyUnit")
                    {
                        //Debug.Log(TargetHit.collider.gameObject);
                        CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                        CurrentActUnitTarget = TargetHit.collider.gameObject;
                    }
                    //Debug.Log(TargetHit.collider.gameObject);
                }
            }
            if (Input.GetKey(KeyCode.Return) && CurrentActUnitTarget != null)
            {
                CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                isWaitForPlayerToChooseUnit = false;
                //StartCoroutine(DealDamage(3f));
                isChooseFinished = true;
                CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                if (CurrentActUnitTarget != null)
                {
                    CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                    CurrentActUnitTarget = null;
                }
                isWaitForPlayerToChooseUnit = false;
                StopAllCoroutines();
                State = BattleState.PlayerTurn;
            }
        }
    }
    /// <summary>
    /// 鼠标选择友方
    /// </summary>
    void ChoosingAlly()
    {
        if (isWaitForPlayerToChooseAlly)
        {
            if (isMoving && !isKeyboardTouched)
            {
                if (CurrentActUnitTarget != null)
                {
                    CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                }
                isKeyboardTouched = false;
                TargetChosenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.Log(TargetChosenRay);
                TargetHit = Physics2D.Raycast(TargetChosenRay.origin, Vector2.down);
                if (ShownUnit != null)
                {
                    if (TargetHit.collider == null || TargetHit.collider.gameObject != ShownUnit)
                    {
                        //Debug.Log("移出collider");
                        //Debug.Log("变没了1");
                        ShownUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                    }
                }
                if (TargetHit.collider != null)
                {
                    //CurrentActUnitTarget = TargetHit.collider.gameObject;
                    //Debug.Log(TargetHit.collider.gameObject.name);
                    //Debug.Log(TargetChosenRay);
                    if (TargetHit.collider.gameObject.tag == "PlayerUnit")
                    {
                        CurrentActUnitTarget = TargetHit.collider.gameObject;
                        ShownUnit = TargetHit.collider.gameObject;
                        //Debug.Log("变了");
                        TargetHit.collider.gameObject.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 255);
                    }

                    if (Input.GetMouseButtonDown(0) && TargetHit.collider.gameObject.tag == "PlayerUnit")
                    {
                        //CurrentActUnitTarget = TargetHit.collider.gameObject;
                        ShownUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        isWaitForPlayerToChooseAlly = false;
                        //StartCoroutine(DealDamage(3f));
                        isChooseFinished = true;
                        CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                    }

                    if (Input.GetKey(KeyCode.Return) && TargetHit.collider.gameObject.tag == "PlayerUnit")
                    {
                        CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        isWaitForPlayerToChooseAlly = false;
                        StartCoroutine(DealDamage(3f));
                        CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                    }
                }
            }
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                isKeyboardTouched = true;
                //增加如果target不存在的检测
                if (CurrentActUnitTarget == null)
                {
                    CurrentActUnitTarget = playerUnits[0];
                }
                CurrentActUnitTarget.GetComponent<Collider2D>().enabled = false;
                //TargetChosenRay = new Ray(CurrentActUnitTarget.transform.position, );
                //TargetHit = Physics2D.Raycast(CurrentActUnitTarget.transform.position, Vector2.down);
                //Debug.Log(TargetHit.collider.gameObject);
                CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 255);

                if (Input.GetButton("Horizontal"))
                {
                    float direction = Input.GetAxisRaw("Horizontal");
                    TargetHit = Physics2D.Raycast(CurrentActUnitTarget.transform.position, new Vector2(direction, 0));
                    if (TargetHit.collider != null && TargetHit.collider.gameObject.tag == "PlayerUnit")
                    {
                        //Debug.Log(TargetHit.collider.gameObject);
                        CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                        CurrentActUnitTarget = TargetHit.collider.gameObject;
                    }
                    //Debug.Log(TargetHit.collider.gameObject);
                }

                if (Input.GetButton("Vertical"))
                {
                    float direction = Input.GetAxisRaw("Vertical");
                    TargetHit = Physics2D.Raycast(CurrentActUnitTarget.transform.position, new Vector2(0, direction));
                    if (TargetHit.collider != null && TargetHit.collider.gameObject.tag == "PlayerUnit")
                    {
                        //Debug.Log(TargetHit.collider.gameObject);
                        CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                        CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                        CurrentActUnitTarget = TargetHit.collider.gameObject;
                    }
                    //Debug.Log(TargetHit.collider.gameObject);
                }
            }
            if (Input.GetKey(KeyCode.Return) && CurrentActUnitTarget != null)
            {
                CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                isWaitForPlayerToChooseAlly= false;
                //StartCoroutine(DealDamage(3f));
                isChooseFinished = true;
                CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                CurrentActUnitTarget.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                CurrentActUnit.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(255, 255, 255, 0);
                if (CurrentActUnitTarget != null)
                {
                    CurrentActUnitTarget.GetComponent<Collider2D>().enabled = true;
                    CurrentActUnitTarget = null;
                }
                isWaitForPlayerToChooseAlly = false;
                StopAllCoroutines();
                State = BattleState.PlayerTurn;
            }
        }
    }
    /// <summary>
    /// 更新血条的函数
    /// </summary>
    public void UpdateSliderChange()
    {
        HpSlider.maxValue = CurrentSliderOwner.GetComponent<GivingData>().maxHP;
        SpSlider.maxValue= CurrentSliderOwner.GetComponent<GivingData>().maxSP;
        HpSlider.value = CurrentSliderOwner.GetComponent<GivingData>().currentHP;
        SpSlider.value = CurrentSliderOwner.GetComponent<GivingData>().currentSP;
    }
    /// <summary>
    /// 检测玩家角色在前排还是后排的函数
    /// </summary>
    public void ComparePosition()
    {
        //完善敌方位置的tag添加
        for (int i = 0; i < PlayerPositionsList.Count; i++) 
        {
            if (PlayerPositionsList[i].transform.childCount != 0)
            {
                if (i < 3)
                {
                    GameObject Character = PlayerPositionsList[i].transform.GetChild(0).gameObject;
                    List<Tag> TagList = Character.GetComponent<GivingData>().tagList;
                    if (TagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        continue;
                    }
                    else if(TagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        TagList.Remove(TagList.Find(tag => tag.TagName == "Remote"));
                    }
                    Character.GetComponent<GivingData>().AddTagToCharacter(Melee.CreateInstance<Melee>());
                    CheckTagList(Character);
                }
                else
                {
                    GameObject Character = PlayerPositionsList[i].transform.GetChild(0).gameObject;
                    List<Tag> TagList = Character.GetComponent<GivingData>().tagList;
                    if (TagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        continue;
                    }
                    else if (TagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        TagList.Remove(TagList.Find(tag => tag.TagName == "Melee"));
                    }
                    Character.GetComponent<GivingData>().AddTagToCharacter(Remote.CreateInstance<Remote>());
                    CheckTagList(Character);
                }
            }
        }

        for (int i = 0; i < EnemyPositionsList.Count; i++)
        {
            if (EnemyPositionsList[i].transform.childCount != 0)
            {
                if (i < 3)
                {
                    GameObject Character = EnemyPositionsList[i].transform.GetChild(0).gameObject;
                    List<Tag> TagList = Character.GetComponent<GivingData>().tagList;
                    if (TagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        continue;
                    }
                    else if (TagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        TagList.Remove(TagList.Find(tag => tag.TagName == "Remote"));
                    }
                    Character.GetComponent<GivingData>().AddTagToCharacter(Melee.CreateInstance<Melee>());
                    CheckTagList(Character);
                }
                else
                {
                    GameObject Character = EnemyPositionsList[i].transform.GetChild(0).gameObject;
                    List<Tag> TagList = Character.GetComponent<GivingData>().tagList;
                    if (TagList.Exists(tag => tag.TagName == "Remote"))
                    {
                        continue;
                    }
                    else if (TagList.Exists(tag => tag.TagName == "Melee"))
                    {
                        TagList.Remove(TagList.Find(tag => tag.TagName == "Melee"));
                    }
                    Character.GetComponent<GivingData>().AddTagToCharacter(Remote.CreateInstance<Remote>());
                    CheckTagList(Character);
                }
            }
        }
    }
    /// <summary>
    /// 更换位置id
    /// </summary>
    public void CheckPositionID()
    {
        for (int i = 0; i < 6; i++)
        {
            if (BattleSetting.Instance.PlayerPositionsList[i].transform.childCount != 0)
            {
                BattleSetting.Instance.PlayerPositionsList[i].transform.GetChild(0).gameObject.GetComponent<GivingData>().positionID = i;
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (BattleSetting.Instance.EnemyPositionsList[i].transform.childCount != 0)
            {
                BattleSetting.Instance.EnemyPositionsList[i].transform.GetChild(0).gameObject.GetComponent<GivingData>().positionID = i;
            }
        }
    }
    /// <summary>
    /// 每个回合结束/开始时进行的操作（目前只有buff的检测和移除） 
    /// </summary>
    void TurnSettle()
    {
        foreach (GameObject unit in BattleUnitsList)
        {
            List<Tag> TagList = new List<Tag>();
            TagList = unit.GetComponent<GivingData>().tagList;

            foreach (Tag tag in TagList)
            {
                if (tag.TagKind == Tag.Kind.turnLessen) 
                {
                    tag.TurnLast--;
                }
            }
            TagList.RemoveAll(tag => tag.TurnLast <= 0 && tag.TagKind == Tag.Kind.turnLessen);
            TagList.RemoveAll(tag => tag.quantity <= 0 && tag.TagKind == Tag.Kind.accumulable);
            CheckTagList(unit);
            isTurnEnding = false;
            //unit.GetComponent<GivingData>().DamageDealMultiplier = 1f;
            //unit.GetComponent<GivingData>().DamageTakeMultiplier = 1f;
        }
        ToBattle();
    }
    /// <summary>
    /// 将buff效果作用在倍率上的函数（有新buff进入list时调用函数使buff即时有效）
    /// </summary>
    public void CheckTagList(GameObject unit)
    {
        unit.GetComponent<GivingData>().PhysicalDamageDealMultiplier = 1f;
        unit.GetComponent<GivingData>().PhysicalDamageTakeMultiplier = 1f;
        unit.GetComponent<GivingData>().SoulDamageDealMultiplier = 1f;
        unit.GetComponent<GivingData>().SoulDamageTakeMultiplier = 1f;

        List<Tag> TagList = unit.GetComponent<GivingData>().tagList;

        foreach (Tag tag in TagList)
        {
            if (tag.Impact == Tag.impactOnMultiplier.PhysicalTake) 
            {
                unit.GetComponent<GivingData>().PhysicalDamageTakeMultiplier *= tag.Multiplier;
            }
            else if(tag.Impact == Tag.impactOnMultiplier.PhysicalDeal)
            {
                unit.GetComponent<GivingData>().PhysicalDamageDealMultiplier *= tag.Multiplier;
            }
            else if (tag.Impact == Tag.impactOnMultiplier.SoulTake)
            {
                unit.GetComponent<GivingData>().SoulDamageTakeMultiplier *= tag.Multiplier;
            }
            else if (tag.Impact == Tag.impactOnMultiplier.SoulDeal)
            {
                unit.GetComponent<GivingData>().SoulDamageDealMultiplier *= tag.Multiplier;
            }
            else if (tag.Impact == Tag.impactOnMultiplier.AllDeal)
            {
                unit.GetComponent<GivingData>().SoulDamageDealMultiplier *= tag.Multiplier;
                unit.GetComponent<GivingData>().PhysicalDamageDealMultiplier *= tag.Multiplier;
            }
            else if (tag.Impact == Tag.impactOnMultiplier.AllTake)
            {
                unit.GetComponent<GivingData>().SoulDamageTakeMultiplier *= tag.Multiplier;
                unit.GetComponent<GivingData>().PhysicalDamageTakeMultiplier *= tag.Multiplier;
            }
        }
    }
    /// <summary>
    /// 更新右侧ui界面
    /// </summary>
    void UpdateUIPanel()
    {
        Avatar.sprite = CurrentActUnit.GetComponent<GivingData>().jobData.JobAvatarImage;
        CurrentSliderOwner = CurrentActUnit;
        SkillID.Clear();
        SkillID.AddRange<int>(CurrentActUnit.GetComponent<GivingData>().jobData.SkillsID);
        UpdateSliderChange();
        CurrentActUnit.GetComponent<JobSkillHolder>().AddSkillToButton();
    }
    /// <summary>
    /// 行动结束回调
    /// </summary>
    public void ActionEnd()
    {
        isWaitForPlayerToChooseUnit = false;
        isWaitForPlayerToChooseAlly = false;
        CurrentActUnit.GetComponent<GivingData>().attackType = AttackType.Null;
        CurrentActUnitTarget = null;
        if (!isActionEnding)
        {
            isActionEnding = true;
            CurrentActUnit.GetComponent<JobSkillHolder>().ActionEndCallback();
        }
    }
    /// <summary>
    /// 检测是否命中
    /// </summary>
    /// <returns></returns>
    bool CheckHit()
    {
        float Hit = Random.Range(0, 1);
        if (HitChanceCounting() <= Hit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 检测是否暴击
    /// </summary>
    /// <returns></returns>
    bool CheckCri()
    {
        float Cri = Random.Range(0, 1);
        if (CriChanceCounting() <= Cri) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 执行一些其他情况的伤害
    /// </summary>
    /// <param name="Damage"></param>
    /// <param name="atkUnit"></param>
    /// <param name="dfsUnit"></param>
    public void DealDamageExtra(int Damage, GameObject atkUnit, GameObject dfsUnit, AttackType attackType)
    {
        //todo: add callback
        if (Damage == -1) 
        {
            Damage = DamageCountingByUnit(atkUnit, dfsUnit, attackType);
        }
        dfsUnit.GetComponent<GivingData>().takeDamage(Damage, attackType);
        //GameStateText.text = "对" + CurrentActUnitTarget.name + "造成伤害" + Damage;
        //StartCoroutine(ShowText(2f));
        CurrentActUnitTarget = null;
    }
    /// <summary>
    /// 检测技能sp是否足够
    /// </summary>
    private void CheckSP()
    {

    }

    public void CheckCanChangeAction()
    {
        if (canChangeAction)
        {
            State = BattleState.PlayerTurn;
            StopAllCoroutines();
            isWaitForPlayerToChooseAlly = false;
            isWaitForPlayerToChooseUnit = false;
            isMoveFinished = true;
            MovePanel.SetActive(false);
            CurrentActUnit.GetComponent<JobSkillHolder>().StopAllCoroutines();
        }
        else
        {
            return;
        }
    }
}
