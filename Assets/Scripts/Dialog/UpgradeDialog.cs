using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDialog : MonoBehaviour
{
    public GameObject upgradeDialog;

    
    bool enter;
    bool enterDialog;
    bool JPressed = false, KPressed = false;
    private void Awake()
    {
        enter = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            JPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            KPressed = true;
        }
    }

    private void FixedUpdate()
    {
        /*if (Input.GetMouseButton(0))
        {
            DataInputs.Instance.playerCurrentLevel = 2;
            DataInputs.Instance.UpdatePlayerStats(DataInputs.Instance.playerCurrentLevel);
        }*/

        if (JPressed && enter && enterDialog) 
        {
            JPressed = false;
            
            if (DataInputs.Instance.playerCurrentLevel <= 9)
            {
                DataInputs.Instance.playerCurrentLevel++;
                DataInputs.Instance.UpdatePlayerStats(DataInputs.Instance.playerCurrentLevel);
            }
            
            
        }
        if (KPressed && enter && enterDialog)
        {
            KPressed = false;
            
            if (DataInputs.Instance.playerCurrentLevel>=2)
            {
                DataInputs.Instance.playerCurrentLevel--;
                DataInputs.Instance.UpdatePlayerStats(DataInputs.Instance.playerCurrentLevel);
            }
            

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            upgradeDialog.SetActive(true);
            enterDialog = true;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            upgradeDialog.SetActive(false);
            enterDialog = false;
        }
    }
}
