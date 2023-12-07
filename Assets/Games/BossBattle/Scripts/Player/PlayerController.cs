using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BossBattle
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void OnPausePressed();
        public static OnPausePressed PausePressed;
        
        [Header("Parameters")]
        [SerializeField] private int _damages = 10;
        [SerializeField] private float _speed = 7f;
        
        [Header("References")]
        [SerializeField] private Animator _animator;

        [Header("Jump Parameters")]
        [SerializeField] private float _jumpPower = 8f;
        [SerializeField] private float _fallMultiplier = 3f;
        [SerializeField] private float _lowJumpMultiplier = 6f;

        private List<HealthSystem> _enemies;
        private Rigidbody2D _rb;
        private BossAI _boss;
        
        private float _fallCache;
        private float _lowJumpCache;
        
        private Vector2 _playerInput = Vector2.zero;
        private Vector2 _playerVelocity = Vector2.zero;
        private bool _playerJumpInput;
        private bool _playerAttackInput;
        private bool _canJump;
        private bool _isCrouching;
        [SerializeField] private bool _isActive;
        
        private static readonly int _IsRunning = Animator.StringToHash("isRunning");
        private static readonly int _IsJumping = Animator.StringToHash("isJumping");
        private static readonly int _IsCrouching = Animator.StringToHash("isCrouching");
        private static readonly int _IsAttacking = Animator.StringToHash("isAttacking");

        public int GetDamages() => _damages;
        public void AddEnemyInRange(HealthSystem enemy) => _enemies.Add(enemy);
        public void RemEnemyInRange(HealthSystem enemy) => _enemies.Remove(enemy);
        public void ResetEnemiesInRange() => _enemies.Clear();
        public List<HealthSystem> GetEnemiesInRange() => _enemies;
        public void SetControlsActive(bool status) => _isActive = status;
        public void ResetVelocity() => _rb.velocity = Vector2.zero;

        public void ResetAnimator()
        {
            _animator.SetBool(_IsRunning, false);
            _animator.SetBool(_IsJumping, false);
            _animator.SetBool(_IsCrouching, false);
            _animator.SetBool(_IsAttacking, false);
        }

        private void Start()
        {
            TryGetComponent(out _rb);
            _enemies = new List<HealthSystem>();
            _fallCache = Physics2D.gravity.y * (_fallMultiplier - 1);
            _lowJumpCache = Physics2D.gravity.y * (_lowJumpMultiplier - 1);
        }

        private void Update()
        {
            if (!_isActive) return;
            
            // Crouching
            _animator.SetBool(_IsCrouching, _isCrouching = (_playerInput.y < 0 && _rb.velocityY == 0));
            
            // Movement
            _playerVelocity = new Vector2(_playerInput.x * _speed, _rb.velocity.y);

            if (!_isCrouching)
            {
                transform.localScale = _rb.velocity.x switch
                {
                    > 0 => new Vector3(1, 1, 1),
                    < 0 => new Vector3(-1, 1, 1),
                    _ => transform.localScale
                };

                _animator.SetBool(_IsRunning, _rb.velocity.x != 0);
            }
            
            else
                _rb.velocity = Vector2.zero;
        }

        void FixedUpdate()
        {
            if (_isCrouching || !_isActive) return;
            
            // Movement
            _rb.velocity = _playerVelocity;
            
            // Jump
            if (_playerJumpInput && _canJump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _jumpPower);
                _animator.SetBool(_IsJumping, true);
                _canJump = false;
            }
            
            if (_rb.velocity.y < 0)
                _rb.velocity += new Vector2(_rb.velocity.normalized.x, _fallCache * Time.fixedDeltaTime);
            
            else if (_rb.velocity.y > 0 && !_playerJumpInput)
                _rb.velocity += new Vector2(_rb.velocity.normalized.x, _lowJumpCache * Time.fixedDeltaTime);

            if (_rb.velocity.y == 0f)
            {
                _canJump = !_playerJumpInput;
                _animator.SetBool(_IsJumping, false);
            }
        }

        [UsedImplicitly] private void OnMovements(InputValue value) => _playerInput = value.Get<Vector2>();
        [UsedImplicitly] private void OnJump(InputValue value) => _playerJumpInput = value.isPressed;

        [UsedImplicitly]
        private void OnPause(InputValue value)
        {
            if (value.isPressed)
                PausePressed?.Invoke();
        }

        [UsedImplicitly] private void OnAttack(InputValue value)
        {
            _playerAttackInput = value.isPressed;
            _animator.SetBool(_IsAttacking, _playerAttackInput);
        }
    }
}