using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples.Example1
{
    public class SimpleExample1Door : SimpleInteractableBase
    {
        public override void BeginInteract()
        {
            ShowText("Press E to open the door.");
        }

        public override void Interact()
        {
            if (SimpleInputHelper.GetKey(KeyCode.E))
            {
                gameObject.SetActive(false);
            }
        }

        public override void EndInteract()
        {
            HideText();
        }
        
    }
}

