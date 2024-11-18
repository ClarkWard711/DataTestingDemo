using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowData : MonoBehaviour
{
    public static ShowData Instance;

    public bool isShowed;
    public bool isPressed;
    public GameObject UpgradePanel, PartyPanel;
    //public Text hp, sp, pa, sa, pd, sd, hit, nim, spd, cri, melee, remote;

    private void Awake()
    {
        Instance = this;
        isShowed = false;
        //isShowed1 = false;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            isPressed = true;
        }
    }

    private void FixedUpdate()
    {
        press();
    }

    void press()
    {
        if (isPressed && !isShowed)
        {
            isShowed = true;
            PartyPanel.SetActive(true);
            //UpgradePanel.GetComponent<CharacterUpgrade>().enabled = false;
            isPressed = false;
        }
        else if (isPressed && isShowed)
        {
            isShowed = false;
            PartyPanel.SetActive(false);
            UpgradePanel.SetActive(false);
            PartyPanel.SetActive(false);
            //UpgradePanel.GetComponent<CharacterUpgrade>().enabled = true;
            isPressed = false;
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
