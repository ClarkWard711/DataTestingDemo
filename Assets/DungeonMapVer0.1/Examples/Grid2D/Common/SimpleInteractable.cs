using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples
{
    public interface SimpleInteractable
    {
        void BeginInteract();
        
        void Interact();
        
        void EndInteract();
        
        bool IsInteractionAllowed();
    }
}

