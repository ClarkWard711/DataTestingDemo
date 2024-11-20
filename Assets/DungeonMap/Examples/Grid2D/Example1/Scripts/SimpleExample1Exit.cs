using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples.Example1
{
    public class SimpleExample1Exit : SimpleInteractableBase
    {

        public override void BeginInteract()
        {
            ShowText("Press E to exit.");
        }

        public override void Interact()
        {
            if (SimpleInputHelper.GetKey(KeyCode.E))
            {
                SimpleExample1GameManager.Instance.LoadNextLevel();
            }
        }

        public override void EndInteract()
        {
            HideText();
        }
    }
}

