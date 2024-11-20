using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples
{
    public static class SimpleInputHelper
    {
        public static bool GetKey(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public static bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public static bool GetKeyUp(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }
        
        public static float GetVerticalAxis()
        {
            if (GetKey(KeyCode.W))
            {
                return 1;
            }

            if (GetKey(KeyCode.S))
            {
                return -1;
            }
            
            return 0;
        }
        
        public static float GetHorizontalAxis()
        {
            if (GetKey(KeyCode.D))
            {
                return 1;
            }

            if (GetKey(KeyCode.A))
            {
                return -1;
            }
            
            return 0;
        }
    }
}

