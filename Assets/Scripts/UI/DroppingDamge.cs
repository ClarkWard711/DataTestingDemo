using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppingDamge : MonoBehaviour
{
    public float speed;
    public float horiSpeed = 0.08f;
    public float acceleration;
    void FixedUpdate()
    {
        //GetComponentInParent<Transform>().transform.Translate(GetComponentInParent<Transform>().transform.position.x, GetComponentInParent<Transform>().transform.position.y + speed, GetComponentInParent<Transform>().transform.position.z);
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (GetComponent<Text>().text != "000")
        {
            speed -= acceleration;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - horiSpeed , rectTransform.anchoredPosition.y + speed);
        }

    }
}
