using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterUpgrade : MonoBehaviour
{
    public static CharacterUpgrade Instance;

    public GameObject UpgradePanel;

    public PartyMember PlayerParty;

    public GameObject PlayerSlotGrid;

    public GameObject emptySlot;

    public List<GameObject> PlayerSlotsList = new List<GameObject>();

    public bool isPressed, isShowed;

    

    #region ShowPanel

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            isPressed = true;
        }
    }

    private void FixedUpdate()
    {
        Press();
    }

    void Press()
    {
        if (isPressed && !isShowed)
        {
            isShowed = true;
            UpgradePanel.SetActive(true);
            isPressed = false;
        }
        else if (isPressed && isShowed)
        {
            isShowed = false;
            UpgradePanel.SetActive(false);
            isPressed = false;
        }
    }

    #endregion

    void Awake()
    {
        if (Instance != null)
            Destroy(this);
        Instance = this;

        isShowed = false;

        UpdateUpgrade();
    }

    public void UpdateUpgrade()
    {
        PlayerSlotsList.Clear();

        while (PlayerSlotGrid.transform.childCount != 0)
        {
            DestroyImmediate(PlayerSlotGrid.transform.GetChild(0).gameObject);
        }

        for (int i = 0; i < 6; i++)
        {
            PlayerSlotsList.Add(Instantiate(emptySlot));
            PlayerSlotsList[i].transform.SetParent(PlayerSlotGrid.transform);
            Instance.PlayerSlotsList[i].GetComponent<Slot>().slotID = i;
            Instance.PlayerSlotsList[i].GetComponent<Slot>().SetupSlots(Instance.PlayerParty.CharacterList[i]);

            Destroy(Instance.PlayerSlotsList[i].GetComponentInChildren<JobOnDrag>());

            if (Instance.PlayerParty.CharacterList[i] != null) 
            {
                Instance.PlayerSlotsList[i].GetComponentInChildren<Button>().interactable = true;
            }
            
        }
    }

    /*void SetupPanel(JobData jobData)
    {
        if (jobData == null)
        {
            return;
        }

        CharacterImage.sprite = jobData.JobAvatarImage;
        CharacterImage.color = new Color(CharacterImage.color.r, CharacterImage.color.g, CharacterImage.color.b, 255);
        JobName.text = jobData.JobName;
    }*/

    
}
