using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Edgar.Unity.Examples
{
    public class SimplePlayerMovement2D : MonoBehaviour
    {
        public float MoveSpeed = 5f;
        
        private Animator animator;
        private Vector2 movement;
        private new Rigidbody2D rigidbody;
        private SpriteRenderer spriteRenderer;

        public void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Update()
        {
            movement.x = SimpleInputHelper.GetHorizontalAxis();
            movement.y = SimpleInputHelper.GetVerticalAxis();
            
            //running
            animator.SetBool("running", movement.magnitude > float.Epsilon);
            
            //Veer
            var flipSprite = spriteRenderer.flipX ? movement.x > 0.01f : movement.x < -0.01f;
            if (flipSprite)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
            
        }

        public void FixedUpdate()
        {
            rigidbody.MovePosition(rigidbody.position + movement.normalized * MoveSpeed * Time.fixedDeltaTime);
        }
    }
}