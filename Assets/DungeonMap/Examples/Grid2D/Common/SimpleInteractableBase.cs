using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Edgar.Unity.Examples
{
    public abstract class SimpleInteractableBase : MonoBehaviour, SimpleInteractable
    {
        protected Text InteractionText;

        public void Start()
        {
            InteractionText = GameObject.Find("Canvas")?.transform.Find("Interaction").GetComponent<Text>();
        }

        protected void ShowText(string text)
        {
            if (InteractionText != null)
            {
                InteractionText.gameObject.SetActive(true);
                InteractionText.text = text;
            }
        }
        
        protected void HideText()
        {
            if (InteractionText != null)
            {
                InteractionText.gameObject.SetActive(false);
            }
        }
        
        
        public virtual void BeginInteract()
        {
            
        }
        
        public virtual void Interact()
        {
            
        }
        
        public virtual void EndInteract()
        {
            
        }

        public virtual bool IsInteractionAllowed()
        {
            return gameObject.activeSelf;
        }

        public void OnDisable()
        {
            EndInteract();
        }
    }
}

