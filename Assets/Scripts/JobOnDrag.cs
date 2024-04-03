using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JobOnDrag : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform originalParent;
    public PartyMember BaseParty;
    public PartyMember PlayerParty;
    private int currentID;


    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        currentID = originalParent.GetComponent<Slot>().slotID;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == null || !eventData.pointerCurrentRaycast.gameObject.CompareTag("Slot"))
        {
            transform.position = originalParent.position;
            transform.SetParent(originalParent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }

        if (eventData.pointerCurrentRaycast.gameObject.name == "JobAvatar")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.position;

            //var tempParty1 = BaseParty;
            //var tempParty2 = PlayerParty;
            if (originalParent.parent.gameObject.CompareTag("BaseParty")) 
            {
                //tempParty1 = BaseParty;
                var temp = BaseParty.CharacterList[currentID];
                if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.CompareTag("BaseParty"))
                {
                    BaseParty.CharacterList[currentID] = BaseParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                    BaseParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;
                    PartyPanel.Instance.UpdatePartyPanel();
                    CharacterUpgrade.Instance.UpdateUpgrade();
                }
                else
                {
                    BaseParty.CharacterList[currentID] = PlayerParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                    PlayerParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;
                    PartyPanel.Instance.UpdatePartyPanel();
                    CharacterUpgrade.Instance.UpdateUpgrade();
                }
                
            }
            else if(originalParent.parent.gameObject.CompareTag("PlayerParty"))
            {
                //tempParty1 = PlayerParty;
                var temp = PlayerParty.CharacterList[currentID];
                if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.CompareTag("BaseParty"))
                {
                    PlayerParty.CharacterList[currentID] = BaseParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                    BaseParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;
                    PartyPanel.Instance.UpdatePartyPanel();
                    CharacterUpgrade.Instance.UpdateUpgrade();
                }
                else
                {
                    PlayerParty.CharacterList[currentID] = PlayerParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID];
                    PlayerParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = temp;
                    PartyPanel.Instance.UpdatePartyPanel();
                    CharacterUpgrade.Instance.UpdateUpgrade();
                }
            }

            eventData.pointerCurrentRaycast.gameObject.transform.position = originalParent.position;
            eventData.pointerCurrentRaycast.gameObject.transform.SetParent(originalParent);
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }

        if (originalParent.parent.gameObject.CompareTag("BaseParty"))
        {
            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.CompareTag("BaseParty"))
            {
                BaseParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = BaseParty.CharacterList[currentID];
                BaseParty.CharacterList[currentID] = null;
                PartyPanel.Instance.UpdatePartyPanel();
                CharacterUpgrade.Instance.UpdateUpgrade();
            }
            else
            {
                PlayerParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = BaseParty.CharacterList[currentID];
                BaseParty.CharacterList[currentID] = null;
                PartyPanel.Instance.UpdatePartyPanel();
                CharacterUpgrade.Instance.UpdateUpgrade();
            }
        }
        else if (originalParent.parent.gameObject.CompareTag("PlayerParty"))
        {
            if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.CompareTag("BaseParty"))
            {
                BaseParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = PlayerParty.CharacterList[currentID];
                PlayerParty.CharacterList[currentID] = null;
                PartyPanel.Instance.UpdatePartyPanel();
                CharacterUpgrade.Instance.UpdateUpgrade();
            }
            else
            {
                PlayerParty.CharacterList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotID] = PlayerParty.CharacterList[currentID];
                PlayerParty.CharacterList[currentID] = null;
                PartyPanel.Instance.UpdatePartyPanel();
                CharacterUpgrade.Instance.UpdateUpgrade();
            }
        }

        transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
        transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    
}
