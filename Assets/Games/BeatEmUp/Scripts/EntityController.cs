using UnityEngine;

namespace Collectif.BeatEmUp
{
    public class EntityController : MonoBehaviour
    {
        [Header("Settings")]
        public float _RunSpeed = 1;
        public float _VerticalMultiplier = 0.8f;
        public bool _FlipSprite = false;

        [Header("References")]
        public GameObject _SpriteGo;
        
        protected Rigidbody2D rb;

        private Animator anim;
        private SpriteRenderer sr;
        private static readonly int animMovementX = Animator.StringToHash("Movement X");
        private static readonly int animMovementY = Animator.StringToHash("Movement Y");

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            sr = _SpriteGo.GetComponent<SpriteRenderer>();
            anim = _SpriteGo.GetComponent<Animator>();
        }

        protected void UpdateSprite()
        {
            FlipSprite();
            AnimatorSprite();
        }

        private void FlipSprite()
        {
            sr.flipX = rb.velocity.x switch
            {
                >= 0.01f => !_FlipSprite,
                <= -0.01f => _FlipSprite,
                _ => sr.flipX
            };
        }

        private void AnimatorSprite()
        {
            anim.SetFloat(animMovementX, rb.velocity.x);
            anim.SetFloat(animMovementY, rb.velocity.y);
        }
    }
}