using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Data;
using System.Linq;

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
    public GameObject MovePanel;//移动选择的panel
    GameObject ShownUnit;//ui展示的单位
    GameObject CurrentSliderOwner;//ui显示单位的slider
    public List<GameObject> PlayerPositionsList;//我方位置
    public List<GameObject> EnemyPositionsList;//敌方位置

    public GameObject SkillList;//基础进阶技能显示
    bool isTextShowed = false;//基础进阶是否显示
    public GameObject BasicPanel, AdvancedPanel;//基础技能和进阶技能的panel
    bool isBasicShowed = false;//基础技能是否显示
    bool isAdvancedShowed = false;//进阶技能是否显示
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

    public bool isWaitForPlayerToChooseAction = false;//等待玩家选择操作
    public bool isWaitForPlayerToChooseUnit = false;//等待玩家选择单位
    public bool isPressed = false;//回主世界是否按下
    bool isMoving = false;//鼠标有没有在移动
    bool isKeyboardTouched = false;//键盘有没有碰 用于键盘操控ui
    public bool isMoveFinished = false;//移动完了没 用于前后排tag
    public BattleState State = BattleState.Start;//战斗进度状态


    bool isChooseFinished = false;//玩家选完了没
    //int TurnCount;

    float alpha;//颜色透明度
    //float DamageMultiplier = 1f;
    public Vector3 Position;

    public Buff Defencing,Charging;//防御 蓄力
    #endregion
    // Start is called before the first frame update
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

        Defencing = new Buff(Buff.Defencing);
        Charging = new Buff(Buff.Charging);
        ComparePosition();
        ListSort();
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
        if (State == BattleState.Won || State == BattleState.Lose && !isPressed) 
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
        ChoosingUnit();
        if (CurrentSliderOwner != null) 
        {
            UpdateSliderChange();
        }
    }

    void ListSort()
    {
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
    }

    void ToBattle()
    {
        RemainingEnemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
        RemainingPlayerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");
        State = BattleState.Middle;

        if (BattleUnitsList.Count == 0) 
        {
            TurnSettle();
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

    IEnumerator DealDamage(float time)
    {
        State = BattleState.Middle;
        float TargetTakeMultiplier, ActUnitDealMultiplier;
        int pa = CurrentActUnit.GetComponent<GivingData>().pa;
        int pd = CurrentActUnitTarget.GetComponent<GivingData>().pd;
        TargetTakeMultiplier = CurrentActUnitTarget.GetComponent<GivingData>().DamageTakeMultiplier;
        ActUnitDealMultiplier = CurrentActUnit.GetComponent<GivingData>().DamageDealMultiplier;
        int Damage = DamageCounting(pa,pd,TargetTakeMultiplier,ActUnitDealMultiplier);
        CurrentActUnitTarget.GetComponent<GivingData>().takeDamage(Damage);
        GameStateText.text = "对" + CurrentActUnitTarget.name + "造成伤害" + Damage;
        StartCoroutine(ShowText(2f));
        CurrentActUnitTarget = null;
        yield return new WaitForSeconds(time);

        ToBattle();
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
        ToBattle();
    }

    IEnumerator Move()
    {
        MovePanel.SetActive(true);
        yield return new WaitUntil(() =>isMoveFinished);
        
        MovePanel.SetActive(false);

        if (State != BattleState.PlayerTurn) 
        {
            GameStateText.text = "移动";
            StartCoroutine(ShowText(2f));
            ComparePosition();
            yield return new WaitForSeconds(2f);
            ToBattle();
        }
        
    }

    IEnumerator Attack()
    {
        yield return new WaitUntil(() => isChooseFinished);
        StartCoroutine(DealDamage(3f));
    }
    #endregion

    #region SetColor
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
            UpdateUIPanel();
            StartCoroutine(TurnAction(1f, "你的回合"));
        }
    }

    #region 玩家按钮对应操作

    public void OnAtkButton()
    {
        if (State != BattleState.PlayerTurn) return;

        isWaitForPlayerToChooseUnit = true;
        StartCoroutine(Attack());
    }

    public void OnDefButton()
    {
        if (State != BattleState.PlayerTurn) return;
        Buff defencing = new Buff(Defencing);
        CurrentActUnit.GetComponent<GivingData>().BuffList.Add(defencing);
        CheckBuffList(CurrentActUnit);
        State = BattleState.Middle;
        StartCoroutine(ShowActionText("防御"));
    }

    public void OnMoveButton()
    {
        if (State != BattleState.PlayerTurn) return;
        isWaitForPlayerToChooseUnit = false;
        CurrentActUnitTarget = null;
        State = BattleState.Middle;
        isMoveFinished = false;
        StartCoroutine(Move());
        
    }

    public void OnChargeButton()
    {
        if (State != BattleState.PlayerTurn) return;
        Buff charging = new Buff(Charging);
        CurrentActUnit.GetComponent<GivingData>().BuffList.Add(charging);
        CheckBuffList(CurrentActUnit);
        State = BattleState.Middle;
        StartCoroutine(ShowActionText("蓄力"));
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

    int DamageCounting(int atk, int dfs, float TakeMultiplier, float DealMultiplier)
    {
        int baseDamage;
        baseDamage = Mathf.CeilToInt(Mathf.CeilToInt((atk * atk) / (atk + dfs)) * TakeMultiplier * DealMultiplier);
        return baseDamage;
    }

    void ChoosingUnit()
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
        for (int i = 0; i < PlayerPositionsList.Count; i++) 
        {
            if (PlayerPositionsList[i].transform.childCount != 0)
            {
                if (i < 3)
                {
                    GameObject Character = PlayerPositionsList[i].transform.GetChild(0).gameObject;
                    List<Buff> BuffList = Character.GetComponent<GivingData>().BuffList;
                    if (BuffList.Contains(Character.GetComponent<GivingData>().Melee))
                    {
                        continue;
                    }
                    else if(BuffList.Contains(Character.GetComponent<GivingData>().Remote))
                    {
                        BuffList.Remove(Character.GetComponent<GivingData>().Remote);
                    }
                    BuffList.Add(Character.GetComponent<GivingData>().Melee);
                }
                else
                {
                    GameObject Character = PlayerPositionsList[i].transform.GetChild(0).gameObject;
                    List<Buff> BuffList = Character.GetComponent<GivingData>().BuffList;
                    if (BuffList.Contains(Character.GetComponent<GivingData>().Remote))
                    {
                        continue;
                    }
                    else if (BuffList.Contains(Character.GetComponent<GivingData>().Melee))
                    {
                        BuffList.Remove(Character.GetComponent<GivingData>().Melee);
                    }
                    BuffList.Add(Character.GetComponent<GivingData>().Remote);
                }
            }
        }
    }
    /// <summary>
    /// 每个回合结束/开始时进行的操作（目前只有buff的检测和移除） 
    /// </summary>
    void TurnSettle()
    {
        BattleUnitsList.AddRange<GameObject>(BattleUnitsListToBeLaunched);
        BattleUnitsListToBeLaunched.Clear();
        foreach (GameObject unit in BattleUnitsList)
        {
            List<Buff> BuffList = new List<Buff>();
            BuffList = unit.GetComponent<GivingData>().BuffList;

            foreach (Buff buff in BuffList)
            {
                if (buff.BuffKind == Buff.Kind.turnLessen) 
                {
                    buff.TurnLast--;
                }

                if (buff.TurnLast == 0 && buff.BuffKind == Buff.Kind.turnLessen)  
                {
                    buff.isTriggered = false;
                }
            }
            BuffList.RemoveAll(buff => buff.isTriggered == false);
            CheckBuffList(unit);
            //unit.GetComponent<GivingData>().DamageDealMultiplier = 1f;
            //unit.GetComponent<GivingData>().DamageTakeMultiplier = 1f;
        }
    }
    /// <summary>
    /// 将buff效果作用在倍率上的函数（有新buff进入list时调用函数使buff即时有效）
    /// </summary>
    public void CheckBuffList(GameObject unit)
    {
        unit.GetComponent<GivingData>().DamageDealMultiplier = 1f;
        unit.GetComponent<GivingData>().DamageTakeMultiplier = 1f;

        List<Buff> BuffList = unit.GetComponent<GivingData>().BuffList;

        foreach (Buff buff in BuffList)
        {
            if (buff.Impact == Buff.impactOnMultiplier.take) 
            {
                unit.GetComponent<GivingData>().DamageTakeMultiplier *= buff.Multiplier;
            }
            else if(buff.Impact == Buff.impactOnMultiplier.deal)
            {
                unit.GetComponent<GivingData>().DamageDealMultiplier *= buff.Multiplier;
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
    }
}
