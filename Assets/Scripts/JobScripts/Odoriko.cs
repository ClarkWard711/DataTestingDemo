using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Odoriko : MonoBehaviour
{
    public Button[] SkillButton;
    void Awake()
    {
        SkillButton = BattleSetting.Instance.SkillList.GetComponentsInChildren<Button>();
        //SkillButton[0].onClick.AddListener(() => Moonlight(BattleSetting.Instance.CurrentActUnit));
    }

    public void Moonlight(GameObject unit)
    {

    }
}
