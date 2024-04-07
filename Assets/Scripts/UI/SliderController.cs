using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public GameObject Character;
    public Slider HpSlider;
    private void Update()
    {
        HpSlider.maxValue = Character.GetComponent<GivingData>().maxHP;
        HpSlider.value = Character.GetComponent<GivingData>().currentHP;
    }
}
