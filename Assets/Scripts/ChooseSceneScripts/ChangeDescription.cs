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
	public void OnPointerEnter(PointerEventData eventData)
	{
		text.text = jobData.Description;
	}
}
