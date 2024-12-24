using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	public GameObject Character;
	public Slider HpSlider;
	public bool isDizzy;
	private void Update()
	{
		if (!isDizzy)
		{
			HpSlider.maxValue = Character.GetComponent<GivingData>().maxHP;
			HpSlider.value = Character.GetComponent<GivingData>().currentHP;
		}
	}
}
