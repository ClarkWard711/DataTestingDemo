using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class ChangePartyMember : MonoBehaviour
{
    public GameObject partyDialog;
    public GameObject characterChoosing;

    public PartyMember BaseParty;
    public PartyMember PlayerParty;

    public GameObject BaseSlotGrid;
    public GameObject PlayerSlotGrid;

    //public Slot slotPrefab;
    public GameObject emptySlot;

    public List<GameObject> BaseSlotsList = new List<GameObject>();
    public List<GameObject> PlayerSlotsList = new List<GameObject>();

    bool enterDialog,isPressed,isShowed;

    static ChangePartyMember instance;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && enterDialog)
        {
            isPressed = true;
            //partyDialog.SetActive(false);
            //characterChoosing.SetActive(true);
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
            partyDialog.SetActive(false);
            characterChoosing.SetActive(true);
            isPressed = false;
        }
        else if (isPressed && isShowed)
        {
            isShowed = false;
            partyDialog.SetActive(true);
            characterChoosing.SetActive(false);
            isPressed = false;
        }
    }

    void Awake()
    {
        if (instance != null)
            Destroy(this);
        instance = this;

        isPressed = false;
        isShowed = false;

        for (int i = 0; i < 6; i++)
        {
            instance.BaseSlotsList.Add(Instantiate(instance.emptySlot));
            instance.BaseSlotsList[i].transform.SetParent(instance.BaseSlotGrid.transform);
            instance.BaseSlotsList[i].GetComponent<Slot>().slotID = i;
            instance.BaseSlotsList[i].GetComponent<Slot>().SetupSlots(instance.BaseParty.CharacterList[i]);
        }

        for (int i = 0; i < 6; i++)
        {
            instance.PlayerSlotsList.Add(Instantiate(instance.emptySlot));
            instance.PlayerSlotsList[i].transform.SetParent(instance.PlayerSlotGrid.transform);
            instance.PlayerSlotsList[i].GetComponent<Slot>().slotID = i;
            instance.PlayerSlotsList[i].GetComponent<Slot>().SetupSlots(instance.PlayerParty.CharacterList[i]);
        }
    }

    /*public static void CreateNewJob(JobData jobData,GameObject Grid)
    {
        Slot newCharacter = Instantiate(instance.slotPrefab,Grid.transform.position,Quaternion.identity);
        newCharacter.gameObject.transform.SetParent(Grid.transform);
        newCharacter.CharacterData = jobData;
        newCharacter.CharacterImage = jobData.JobAvatarImage;
        newCharacter.JobName.text = jobData.JobName;
    }*/

    #region ShowPanel
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            partyDialog.SetActive(true);
            enterDialog = true;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            partyDialog.SetActive(false);
            characterChoosing.SetActive(false);
            enterDialog = false;
        }
    }
    #endregion
}
