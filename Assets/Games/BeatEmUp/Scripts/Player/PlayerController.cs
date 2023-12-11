using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeatEmUp
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void InputPressed();
        public static InputPressed OnPausePressed;
        
        [Header("Parameters")]
        [SerializeField] private Transform _playerSprites;
        [SerializeField] private float _speed = 5.0f;
        
        [Header("VFX")]
        [SerializeField] private GameObject _blockEffect;
        [SerializeField] private GameObject _damageEffect;
        
        [Space()]
        [SerializeField] private Transform _blockEffectPosition;
        [SerializeField] private Transform _damageEffectPosition;
        
        [Header("SFX")]
        [SerializeField] private AudioClip _hitSfx;
        [SerializeField] private AudioClip _hitLandSfx;
        [SerializeField] private AudioClip _stickHitSfx;
        [SerializeField] private AudioClip _stickHitLandSfx;
        [SerializeField] private AudioClip _shieldBlockSfx;
        
        [Space()]
        [SerializeField] private AudioClip _damageSfx;
        [SerializeField] private AudioClip _deathSfx;

        private PlayerSpriteController _playerSpriteController;
        private AudioSource _audioSource;
        private DamageSystem _damageSystem;
        private HealthSystem _health;
        private Animator _animator;
        private Rigidbody2D _rb;

        private List<HealthSystem> _enemiesInRange;
        private Vector2 _playerInput = Vector2.zero;
        private Vector2 _playerVelocity = Vector2.zero;
        private bool _playerAttack;
        private bool _canMove = true;
        private bool _isProtecting;
        private bool _hasShield;
        private Pickable _pickUp;

        private static readonly int IsRun = Animator.StringToHash("isRun");
        private static readonly int IsDamaged = Animator.StringToHash("isDamaged");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsLooting = Animator.StringToHash("isLooting");
        private static readonly int IsDefending = Animator.StringToHash("isDefending");
        private static readonly int HasDefenseHit = Animator.StringToHash("hasDefenseHit");
        private static readonly int HasStick = Animator.StringToHash("hasStick");

        public Rigidbody2D GetRb() => _rb;
        public List<HealthSystem> GetEnemiesInRange() => _enemiesInRange;
        public void AddEnemyInRange(HealthSystem enemy) => _enemiesInRange.Add(enemy);
        public void RemEnemyInRange(HealthSystem enemy) => _enemiesInRange.Remove(enemy);
        public void CanPlayerMove(bool canMove) => _canMove = canMove;
        public int GetDamageDealt() => _damageSystem.GetDamageDealt();
        public void SetPickup(Pickable pickUp) => _pickUp = pickUp;
        public void Anim_PickupItem()  { if (_pickUp != null) _playerSpriteController.OnPickup(_pickUp.GetPickup()); }
        public void DisablePickable() { if (_pickUp != null) _pickUp.gameObject.SetActive(false); }
        public void GetShield() => _hasShield = true;
        public AudioClip GetMissedSfx() => _animator.GetBool(HasStick) ? _stickHitSfx : _hitSfx;
        public AudioClip GetLandedSfx() => _animator.GetBool(HasStick) ? _stickHitLandSfx : _hitLandSfx;

        private void Start()
        {
            _enemiesInRange = new List<HealthSystem>();
            _playerSprites.TryGetComponent(out _animator);
            
            TryGetComponent(out _playerSpriteController);
            TryGetComponent(out _damageSystem);
            TryGetComponent(out _audioSource);
            TryGetComponent(out _health);
            TryGetComponent(out _rb);

            _health.InitializeSound(_damageSfx, _deathSfx);

            _hasShield = _playerSpriteController.HasShield();

            _health.Initialize(_damageSfx, _deathSfx);
        }

        public void Attacked(int damages)
        {
            if (_isProtecting)
            {
                _audioSource.PlayOneShot(_shieldBlockSfx);
                Instantiate(_blockEffect, _blockEffectPosition.position, Quaternion.identity);
                _animator.SetTrigger(HasDefenseHit);
            }

            else if (!_health.IsInvuln())
            {
                Instantiate(_damageEffect, _damageEffectPosition.position, Quaternion.identity);
                _animator.SetTrigger(IsDamaged);
                _health.TakeDamage(damages);
            }
        }

        private void PickupItem()
        {
            if (_pickUp == null) return;
            _animator.SetTrigger(IsLooting);
        }

        private void Update()
        {
            _animator.SetBool(IsDefending, _isProtecting);
            
            _playerVelocity = _canMove ? _playerInput * _speed : Vector2.zero;

            if (_playerVelocity.x > 0)
                transform.localScale = new Vector3( 1, 1, 1);
            else if (_playerVelocity.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            if (_playerVelocity.x != 0 || _playerVelocity.y != 0)
                _animator.SetBool(IsRun, true);
            else
                _animator.SetBool(IsRun, false);
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + _playerVelocity * Time.fixedDeltaTime);
        }

        [UsedImplicitly] public void OnMovements(InputValue value) => _playerInput = value.Get<Vector2>();
        [UsedImplicitly] public void OnAttack(InputValue value)
        {
            if (!value.isPressed) return;
            _animator.SetTrigger(IsAttacking);
        }
        
        [UsedImplicitly] public void OnPickupItem(InputValue value)
        {
            if (!value.isPressed) return;
            PickupItem();
        }
        
        [UsedImplicitly] public void OnProtect(InputValue value)
        {
            if (!_hasShield) return;
            _canMove = !value.isPressed;
            _isProtecting = value.Get<float>() > 0.5f;
        }
        
        [UsedImplicitly] public void OnPause(InputValue value)
        {
            if (value.isPressed)
                OnPausePressed?.Invoke();
        }
    }
}