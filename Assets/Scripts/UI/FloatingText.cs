using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FloatingText : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerMoveHandler
{
    public GameObject TextPanelPrefab;
    public GameObject TextPanel;
    public Canvas canvas;
    public void OnPointerEnter(PointerEventData eventData)
    {
        TextPanel = Instantiate(TextPanelPrefab, canvas.transform);
        //TextPanel.transform.position = eventData.position;
        //TextPanel.GetComponentInChildren<Text>().text = "";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject.Destroy(TextPanel);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        TextPanel.transform.position = eventData.position;
    }
}
