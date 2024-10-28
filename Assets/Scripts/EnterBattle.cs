using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EnterBattle : MonoBehaviour
{
    #region 一堆public
    public static EnterBattle Instance;

    [SerializeField] AssetReference outerScene;
    
    public int cHP;
    public int cSP;
    public int maxHP;
    public int maxSP;
    public int pa;
    public int sa;
    public int pd;
    public int sd;
    public int hit;
    public int nim;
    public int spd;
    public int cri;
    public int melee;
    public int remote;
    public int deltaExp;
    public int deltaSp;

    public int enemyPhysicalAtk, enemyPhysicalDfs;
    public int enemySoulAtk,enemySoulDfs;

    public int basePlayerPhysicalDamage;
    public int basesPlayerSoulDamage;
    public int baseEnemyPhysicalDamage;
    public int baseEnemySoulDamage;

    public float EnemyHittingChance;
    public float PlayerHittingChance;

    public GameObject StatePanel;
    public GameObject SkillList;
    public Text GameStateText;
    public Text EnemyDamageText,PlayerDamageText;
    public Text EnemyHealText,PlayerHealText;
    public Slider HpSlider;
    public Slider SpSlider;
    public Slider DizzinessSlider;
    public float Speed;
    float alpha;
    bool isTextShowed;
    bool isPressed;
    Vector2 transform1, transform2, transform3, transform4;
    #endregion

    void Awake()
    {
        isPressed = false;
        GameObject.Find("Player").SetActive(false);
        Instance = this;
        SkillList.SetActive(false);
        /*
        Debug.Log(SaveAndLoad.Instance.playerCurrentLevel);
        SaveAndLoad.Instance.playerCurrentLevel = 2;
        Debug.Log(SaveAndLoad.Instance.playerCurrentLevel);
        */
        transform1 = EnemyDamageText.rectTransform.anchoredPosition;
        transform2 = PlayerDamageText.rectTransform.anchoredPosition;
        transform3 = EnemyHealText.rectTransform.anchoredPosition;
        transform4 = PlayerHealText.rectTransform.anchoredPosition;

        HpSlider.maxValue = maxHP;
        SpSlider.maxValue = maxSP;

        HpSlider.value = cHP;
        SpSlider.value = cSP;

        basePlayerPhysicalDamage = Damage(pa, enemyPhysicalDfs);//多角色伤害问题在此版本尚未解决
        basesPlayerSoulDamage = Damage(sa, enemySoulDfs);
        baseEnemyPhysicalDamage = Damage(enemyPhysicalAtk, pd);
        baseEnemySoulDamage = Damage(enemySoulAtk, sd);
        
        GameStateText.text = "对战开始";
        SetColorTo0(GameStateText);
        SetColorTo0(EnemyDamageText);
        SetColorTo0(EnemyHealText);
        StartCoroutine(ShowText());
        
    }

    #region SetColor
    void SetColorTo0(Text text)
    {
        text.color = new Color(text.color.r, text.color.b, text.color.g, 0);
    }


    void SetColorTo1(Text text)
    {
        text.color = new Color(text.color.r, text.color.b, text.color.g, 1);
        //alpha = 1;放在造成伤害之后
    }

    void SetColorTo0Gradually(Text text)
    {
        if (alpha>0)
        {
            alpha -= 0.01f;
        }
        text.color = new Color(text.color.r, text.color.b, text.color.g, alpha);
    }
    #endregion

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPressed )
        {
            isPressed = true;
            SceneLoader.LoadAddressableScene(outerScene);
        }
        SetColorTo0Gradually(EnemyDamageText);
        SetColorTo0Gradually(PlayerDamageText);
        SetColorTo0Gradually(PlayerHealText);
        SetColorTo0Gradually(EnemyHealText);

    }

    int Damage(int atk, int dfs)
    {
        int baseDamage;
        baseDamage = Mathf.CeilToInt((atk * atk) / (atk + dfs));
        return baseDamage;
        
    }

    

    IEnumerator ShowText()
    {
        SetColorTo1(GameStateText);
        StatePanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        StatePanel.SetActive(false);
        SetColorTo0(GameStateText);
    }

    public void HideList()
    {
        if (!isTextShowed)
        {
            SkillList.SetActive(false);
            isTextShowed = true;
        }
        else
        {
            SkillList.SetActive(true);
            isTextShowed = false;
        }
        
    }


    void ChanceOfHitting()
    {

    }

    #region Floating
    void Float(Text text)
    {
        text.rectTransform.anchoredPosition = new Vector2(text.rectTransform.anchoredPosition.x, text.rectTransform.anchoredPosition.y + Speed);
    }

    void ResetTransform()
    {
        EnemyDamageText.rectTransform.anchoredPosition = transform1;
        PlayerDamageText.rectTransform.anchoredPosition = transform2;
        EnemyHealText.rectTransform.anchoredPosition = transform3;
        PlayerHealText.rectTransform.anchoredPosition = transform4;
    }
    #endregion
}
