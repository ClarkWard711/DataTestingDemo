using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour
{
    public int slotID;

    public JobData CharacterData;
    public Image CharacterImage;
    public Text JobName;

    public GameObject slot;

    public void SetupSlots(JobData jobData)
    {
        if (jobData == null)
        {
            return;
        }

        CharacterImage.sprite = jobData.JobAvatarImage;
        CharacterImage.color = new Color(CharacterImage.color.r, CharacterImage.color.g, CharacterImage.color.b, 255);
        JobName.text = jobData.JobName;
    }

    public void ChooseCharacter()
    {
        //GameObject click = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(click);
        //Debug.Log(1);
        //click.GetComponent<Image>().color = new Color(click.GetComponent<Image>().color.r, click.GetComponent<Image>().color.g, click.GetComponent<Image>().color.b, 175);
        //CharacterImage.color = new Color(CharacterImage.color.r, CharacterImage.color.g, CharacterImage.color.b, 50);
        
        ChoosingCharacter.Instance.ChangeChosenCharacter(slotID);
    }
}
