using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edgar.Unity.Examples
{
    public class SimpleCameraController : MonoBehaviour
    {
        public float ZoomOutSize = 70;
        private new Camera camera;
        private GameObject player;
        
        private bool isZoomedOut;
        private float previousOrthographicSize;
        private Vector3 previousPosition;

        public void Start()
        {
            camera = GetComponent<Camera>();
        }
        public void Update()
        {
            if (SimpleInputHelper.GetKeyDown(KeyCode.LeftControl))
            {
                previousOrthographicSize = camera.orthographicSize;
                camera.orthographicSize = ZoomOutSize;
                
                previousPosition = transform.position; 
                transform.position = new Vector3(0, 0, previousPosition.z);
                isZoomedOut = true;
            }

            if (SimpleInputHelper.GetKeyUp(KeyCode.LeftControl))
            {
                camera.orthographicSize = previousOrthographicSize;
                transform.position = previousPosition;
                isZoomedOut = false;
            }
            
        }

        public void LateUpdate()
        {
            if (player == null)
            {
                player = GameObject.FindWithTag("Player");
            }

            if (player != null && !isZoomedOut)
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            }
        }
    }

}
