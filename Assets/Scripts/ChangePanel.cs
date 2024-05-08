using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePanel : MonoBehaviour
{

    public GameObject ChangePagePanel, UpgradePanel, DataPanel, PartyPanel;

    void Update()
    {
        if (ShowData.Instance.isShowed == true) 
        {
            ChangePagePanel.SetActive(true);
        }
        else
        {
            ChangePagePanel.SetActive(false);
            CharacterUpgrade.Instance.isShowed = false;
        }
    }

    public void Party()
    {
        PartyPanel.SetActive(true);
        UpgradePanel.SetActive(false);
        DataPanel.SetActive(false);
        CharacterUpgrade.Instance.isShowed = false;
    }

    public void Data()
    {
        PartyPanel.SetActive(false);
        UpgradePanel.SetActive(false);
        DataPanel.SetActive(true);
        CharacterUpgrade.Instance.isShowed = false;
    }

    public void Upgrade()
    {
        PartyPanel.SetActive(false);
        UpgradePanel.SetActive(true);
        DataPanel.SetActive(false);
        CharacterUpgrade.Instance.isShowed = true;
    }
}
