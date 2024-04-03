using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamage : MonoBehaviour
{
    public float speed;
    void FixedUpdate()
    {
        //GetComponentInParent<Transform>().transform.Translate(GetComponentInParent<Transform>().transform.position.x, GetComponentInParent<Transform>().transform.position.y + speed, GetComponentInParent<Transform>().transform.position.z);
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (GetComponent<Text>().text!="000")
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + speed * 0.1f);
        }
        
    }
}
