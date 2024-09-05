using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    bool isPressed;
    public static PositionControl Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPressed)
        {
            isPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (isPressed)
        {
            isPressed = false;
            BackToAction();
        }
    }

    public void Button1()
    {
        PositionControlCore(0);
    }
    public void Button2()
    {
        PositionControlCore(1);
    }
    public void Button3()
    {
        PositionControlCore(2);
    }
    public void Button4()
    {
        PositionControlCore(3);
    }
    public void Button5()
    {
        PositionControlCore(4);
    }
    public void Button6()
    {
        PositionControlCore(5);
    }

    void BackToAction()
    {
        BattleSetting.Instance.State = BattleState.PlayerTurn;
        BattleSetting.Instance.isMoveFinished = true;
    }

    public void PositionControlCore(int i)
    {
        if (BattleSetting.Instance.PlayerPositionsList[i].transform.childCount != 0)
        {
            if (BattleSetting.Instance.CurrentActUnit == BattleSetting.Instance.PlayerPositionsList[i].transform.GetChild(0).gameObject)
            {
                return;
            }
            else
            {
                BattleSetting.Instance.PlayerPositionsList[i].transform.GetChild(0).gameObject.transform.SetParent(BattleSetting.Instance.CurrentActUnit.transform.parent, false);
                BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[i].transform, false);
                BattleSetting.Instance.isMoveFinished = true;
            }
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[i].transform, false);
            BattleSetting.Instance.isMoveFinished = true;
        }
        BattleSetting.Instance.CheckPositionID();
        BattleSetting.Instance.ComparePosition();
    }
}
