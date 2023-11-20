using System;
using UnityEngine;

namespace Collectif.BeatEmUp {

    public class Player : Entity
    {
        [Header("Player Parameters")]
        
        public Transform _LeftTransform;
        public Transform _RightTransform;
        private float movementX, movementY;

        protected void Update()
        {
            UpdateSprite();
            movementX = Input.GetAxisRaw("Horizontal");
            movementY = Input.GetAxisRaw("Vertical");
        }
        
        private void FixedUpdate()
        {
            var movement = new Vector2(
                movementX * (_RunSpeed * 100),
                (movementY * (_RunSpeed * 100)) * _VerticalMultiplier);

            rb.velocity = movement * Time.deltaTime;
        }
    }
    
}