using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeDescription : MonoBehaviour, IPointerEnterHandler
{
	public JobData jobData;
	public Text text;
	public bool isChosen;
	public int index;
	public bool isSkill;
	public int jobID;
	public string description = "";
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!isSkill && jobData != null)
		{
			text.text = jobData.Description;
		}
		else
		{
			text.text = description;
		}
	}
}
