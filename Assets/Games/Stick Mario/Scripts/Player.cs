using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collectif.Mario
{
    public class Player : MonoBehaviour
    {
        public float _RunSpeed = 3f;
        public float _JumpPower = 3f;
        public float _FallMultiplier = 2.5f;
        public float _LowJumpMultiplier = 2f;

        public Transform _GroundCheck;
        public LayerMask _GroundLayer;
        
        private bool isGrounded;
        private float movementX;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            movementX = Input.GetAxisRaw("Horizontal");
            
            if (Input.GetButtonDown("Jump") && IsGrounded())
                rb.velocity = new Vector2(rb.velocity.x, _JumpPower);
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2( movementX * _RunSpeed, rb.velocity.y);
            
            if (rb.velocity.y < 0)
                rb.velocity += Vector2.up * Physics2D.gravity.y * (_FallMultiplier - 1) * Time.deltaTime;
            
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
                rb.velocity += Vector2.up * Physics2D.gravity.y * (_LowJumpMultiplier - 1) * Time.deltaTime;
        }
        
        private bool IsGrounded()
        {
            return Physics2D.OverlapBox(_GroundCheck.position, new Vector2(0.45f, 0.45f), 0, _GroundLayer);
        }
    }
}