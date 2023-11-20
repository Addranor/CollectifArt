using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collectif.BeatEmUp
{
    public class Entity : MonoBehaviour
    {
        [Header("Settings")]
        public float _RunSpeed = 1;
        public float _VerticalMultiplier = 0.8f;
        public bool _FlipSprite = false;

        [Header("References")]
        public GameObject _SpriteGo;
        public BoxCollider2D _SensorTrigger;
        
        protected Rigidbody2D rb;

        private Animator anim;
        private SpriteRenderer sr;
        private static readonly int animMovementX = Animator.StringToHash("Movement X");
        private static readonly int animMovementY = Animator.StringToHash("Movement Y");
        
        private static readonly int animIsHitting = Animator.StringToHash("IsHitting");
        private static readonly int animOnGround = Animator.StringToHash("OnGround");
        private static readonly int animDamaged = Animator.StringToHash("Damaged");
        private static readonly int animThrown = Animator.StringToHash("Thrown");

        private Vector2 entityTriggerOffset;
        protected bool isPlayerHere = false;
        protected List<Entity> enemies;

        public bool IsPlayerHere
        {
            get => isPlayerHere;
            set => isPlayerHere = value;
        }

        public void AddEnemy(Entity enemy) => enemies.Add(enemy);
        public void RemoveEnemy(Entity enemy) => enemies.Remove(enemy);
        public void ClearEnemies() => enemies.Clear();

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = _SpriteGo.GetComponent<SpriteRenderer>();
            anim = _SpriteGo.GetComponent<Animator>();

            entityTriggerOffset = _SensorTrigger.offset;
            enemies = new List<Entity>();
        }

        protected void UpdateSprite()
        {
            FlipSprite();
            AnimatorSprite();
        }

        protected void FlipSprite()
        {
            switch (rb.velocity.x)
            {
                case (>= 0.01f):
                    sr.flipX = !_FlipSprite;
                    _SensorTrigger.offset = entityTriggerOffset;
                    break;
                
                case (<= -0.01f):
                    sr.flipX = _FlipSprite;
                    _SensorTrigger.offset = Vector2.zero - entityTriggerOffset;
                    break;
            }
        }

        protected void ForceFlipSprite()
        {
            sr.flipX = !sr.flipX;
            _SensorTrigger.offset = entityTriggerOffset * -1;
        }

        private void AnimatorSprite()
        {
            anim.SetFloat(animMovementX, rb.velocity.x);
            anim.SetFloat(animMovementY, rb.velocity.y);
        }
        
        protected void Attack()
        {
            Debug.Log("Attack");
        }

        protected void ToggleControl(bool overrideToggle = false)
        {
            
        }
    }
}