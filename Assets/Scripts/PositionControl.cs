using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionControl : MonoBehaviour
{
    bool isPressed;
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
        if (BattleSetting.Instance.PlayerPositionsList[0].transform.childCount != 0)  
        {
            if (BattleSetting.Instance.CurrentActUnit == BattleSetting.Instance.PlayerPositionsList[0].transform.GetChild(0).gameObject)
            {
                
                return;
            }
            else
            {
                BattleSetting.Instance.PlayerPositionsList[0].transform.GetChild(0).gameObject.transform.SetParent(BattleSetting.Instance.CurrentActUnit.transform.parent, false);
                BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[0].transform, false);
                BattleSetting.Instance.isMoveFinished = true;
            }
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[0].transform, false);
            BattleSetting.Instance.isMoveFinished = true;
        }
    }
    public void Button2()
    {
        if (BattleSetting.Instance.PlayerPositionsList[1].transform.childCount != 0)
        {
            if (BattleSetting.Instance.CurrentActUnit == BattleSetting.Instance.PlayerPositionsList[1].transform.GetChild(0).gameObject)
            {
                
                return;
            }
            else
            {
                BattleSetting.Instance.PlayerPositionsList[1].transform.GetChild(0).gameObject.transform.SetParent(BattleSetting.Instance.CurrentActUnit.transform.parent, false);
                BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[1].transform, false);
                BattleSetting.Instance.isMoveFinished = true;
            }
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[1].transform, false);
            BattleSetting.Instance.isMoveFinished = true;
        }
    }
    public void Button3()
    {
        if (BattleSetting.Instance.PlayerPositionsList[2].transform.childCount != 0)
        {
            if (BattleSetting.Instance.CurrentActUnit == BattleSetting.Instance.PlayerPositionsList[2].transform.GetChild(0).gameObject)
            {
                
                return;
            }
            else
            {
                BattleSetting.Instance.PlayerPositionsList[2].transform.GetChild(0).gameObject.transform.SetParent(BattleSetting.Instance.CurrentActUnit.transform.parent, false);
                BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[2].transform, false);
                BattleSetting.Instance.isMoveFinished = true;
            }
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[2].transform, false);
            BattleSetting.Instance.isMoveFinished = true;
        }
    }
    public void Button4()
    {
        if (BattleSetting.Instance.PlayerPositionsList[3].transform.childCount != 0)
        {
            if (BattleSetting.Instance.CurrentActUnit == BattleSetting.Instance.PlayerPositionsList[3].transform.GetChild(0).gameObject)
            {
                
                return;
            }
            else
            {
                BattleSetting.Instance.PlayerPositionsList[3].transform.GetChild(0).gameObject.transform.SetParent(BattleSetting.Instance.CurrentActUnit.transform.parent, false);
                BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[3].transform, false);
                BattleSetting.Instance.isMoveFinished = true;
            }
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[3].transform, false);
            BattleSetting.Instance.isMoveFinished = true;
        }
    }
    public void Button5()
    {
        if (BattleSetting.Instance.PlayerPositionsList[4].transform.childCount != 0)
        {
            if (BattleSetting.Instance.CurrentActUnit == BattleSetting.Instance.PlayerPositionsList[4].transform.GetChild(0).gameObject)
            {
                
                return;
            }
            else
            {
                BattleSetting.Instance.PlayerPositionsList[4].transform.GetChild(0).gameObject.transform.SetParent(BattleSetting.Instance.CurrentActUnit.transform.parent, false);
                BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[4].transform, false);
                BattleSetting.Instance.isMoveFinished = true;
            }
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[4].transform, false);
            BattleSetting.Instance.isMoveFinished = true;
        }
    }
    public void Button6()
    {
        if (BattleSetting.Instance.PlayerPositionsList[5].transform.childCount != 0)
        {
            if (BattleSetting.Instance.CurrentActUnit == BattleSetting.Instance.PlayerPositionsList[5].transform.GetChild(0).gameObject)
            {
                
                return;
            }
            else
            {
                BattleSetting.Instance.PlayerPositionsList[5].transform.GetChild(0).gameObject.transform.SetParent(BattleSetting.Instance.CurrentActUnit.transform.parent, false);
                BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[5].transform, false);
                BattleSetting.Instance.isMoveFinished = true;
            }
        }
        else
        {
            BattleSetting.Instance.CurrentActUnit.transform.SetParent(BattleSetting.Instance.PlayerPositionsList[5].transform, false);
            BattleSetting.Instance.isMoveFinished = true;
        }
    }

    void BackToAction()
    {
        BattleSetting.Instance.State = BattleState.PlayerTurn;
        BattleSetting.Instance.isMoveFinished = true;
    }
}
