using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples
{
    public class SimplePlayer2D : MonoBehaviour
    {
        private SimpleInteractable interactableInFocus;

        public void Update()
        {
            if (interactableInFocus != null)
            {
                if (interactableInFocus.IsInteractionAllowed())
                {
                    interactableInFocus.Interact();
                }
                else
                {
                    Debug.Log("SimplePlayer2D interaction disabled");
                    interactableInFocus.EndInteract();
                    interactableInFocus = null;
                }
            }
            
        }


        public void OnTriggerEnter2D(Collider2D collider)
        {
            var interactable = collider.GetComponent<SimpleInteractable>();
            if (interactable == null || !interactable.IsInteractionAllowed())
            {
                Debug.Log("SimplePlayer2D--OnTriggerEnter2D-- interactable is null");
                return;
            }
            interactableInFocus?.EndInteract();
            interactableInFocus = interactable;
            interactableInFocus.BeginInteract();
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            var interactable = collider.GetComponent<SimpleInteractable>();

            if (interactable == interactableInFocus)
            {
                interactableInFocus?.EndInteract();
                interactableInFocus = null;
            }
        }
    }
}

