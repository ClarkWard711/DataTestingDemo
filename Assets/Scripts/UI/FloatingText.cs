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

    /*private void Update()
    {
        if (!BattleSetting.Instance.canShowDescription)
        {
            if (TextPanel != null)
            {
                GameObject.Destroy(TextPanel);
            }
        }
    }*/
    public void OnPointerEnter(PointerEventData eventData)
    {
        TextPanel = Instantiate(TextPanelPrefab, this.transform);
        //TextPanel.transform.position = eventData.position;
        //TextPanel.GetComponentInChildren<Text>().text = "";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyPanel();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (TextPanel == null)
        {
            TextPanel = Instantiate(TextPanelPrefab, this.transform);
        }
        TextPanel.transform.position = eventData.position;
    }

    public void DestroyPanel()
    {
        if (TextPanel != null) 
        {
            GameObject.Destroy(TextPanel);
        }
        
    }
}
