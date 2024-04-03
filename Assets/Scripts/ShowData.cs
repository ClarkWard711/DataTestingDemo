using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowData : MonoBehaviour
{
    public static ShowData Instance;

    public bool isShowed,isShowed1;
    public bool isPressed,isOPressed;
    public GameObject DataPanel,PartyPanel;
    //public Text hp, sp, pa, sa, pd, sd, hit, nim, spd, cri, melee, remote;

    private void Awake()
    {
        Instance = this;
        isShowed = false;
        isShowed1 = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I)&&!isShowed1)
        {
            isPressed = true;
        }
        else if (Input.GetKeyUp(KeyCode.O)&&!isShowed)
        {
            isOPressed = true;
        }
    }

    private void FixedUpdate()
    {
        press();
    }

    void press()
    {
        if (isPressed && !isShowed && !isShowed1)
        {
            isShowed = true;
            DataPanel.SetActive(true);
            isPressed = false;
        }
        else if (isPressed && isShowed && !isShowed1) 
        {
            isShowed = false;
            DataPanel.SetActive(false);
            isPressed = false;
        }

        if (isOPressed && !isShowed && !isShowed1)
        {
            isShowed1 = true;
            PartyPanel.SetActive(true);
            isOPressed = false;
        }
        else if (isOPressed && !isShowed && isShowed1) 
        {
            isShowed1 = false;
            PartyPanel.SetActive(false);
            isOPressed = false;
        }
    }

    /*public void UpdateDataPanel()
    {
        hp.text = DataInputs.Instance.maxHPOfPlayer.ToString();
        sp.text = DataInputs.Instance.maxSPOfPlayer.ToString();
        pa.text = DataInputs.Instance.physicalAttackOfPlayer.ToString();
        sa.text = DataInputs.Instance.soulAttackOfPlayer.ToString();
        pd.text = DataInputs.Instance.physicalDefenceOfPlayer.ToString();
        sd.text = DataInputs.Instance.soulDefenceOfPlayer.ToString();
        hit.text = DataInputs.Instance.hitOfPlayer.ToString();
        nim.text = DataInputs.Instance.nimblenessOfPlayer.ToString();
        spd.text = DataInputs.Instance.speedOfPlayer.ToString();
        cri.text = DataInputs.Instance.criticalOfPlayer.ToString();
        melee.text = DataInputs.Instance.meleeOfPlayer.ToString();
        remote.text = DataInputs.Instance.remoteOfPlayer.ToString();
    }*/
}
