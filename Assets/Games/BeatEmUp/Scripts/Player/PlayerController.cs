using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BeatEmUp
{
    public class PlayerController : MonoBehaviour
    {
        public delegate void InputPressed();
        public delegate void OnPickup(PickUps pickup);
        public static InputPressed OnPausePressed;
        public static OnPickup OnPickUp;
        
        [SerializeField] private Transform _playerSprites;
        [SerializeField] private float _speed = 5.0f;

        private Rigidbody2D _rb;
        private Animator _animator;
        private DamageSystem _damageSystem;
        private HealthSystem _health;

        private List<HealthSystem> _enemiesInRange;
        private Vector2 _playerInput = Vector2.zero;
        private Vector2 _playerVelocity = Vector2.zero;
        private bool _playerAttack;
        private bool _canMove = true;

        private static readonly int IsRun = Animator.StringToHash("isRun");
        private static readonly int IsDamaged = Animator.StringToHash("isDamaged");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");

        public Rigidbody2D GetRb() => _rb;
        public List<HealthSystem> GetEnemiesInRange() => _enemiesInRange;
        public void AddEnemyInRange(HealthSystem enemy) => _enemiesInRange.Add(enemy);
        public void RemEnemyInRange(HealthSystem enemy) => _enemiesInRange.Remove(enemy);
        public void CanPlayerMove(bool canMove) => _canMove = canMove;
        public int GetDamageDealt() => _damageSystem.GetDamageDealt();

        private void Start()
        {
            _enemiesInRange = new List<HealthSystem>();
            _playerSprites.TryGetComponent(out _animator);
            TryGetComponent(out _damageSystem);
            TryGetComponent(out _health);
            TryGetComponent(out _rb);

            _health.OnDamage += StaggerHit;
        }

        private void OnDisable()
        {
            _health.OnDamage -= StaggerHit;
        }

        private void StaggerHit(int amount)
        {
            _animator.SetTrigger(IsDamaged);
        }

        private void Update()
        {
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
            if (value.isPressed)
                _animator.SetTrigger(IsAttacking);
        }
        
        [UsedImplicitly] public void OnPause(InputValue value)
        {
            if (value.isPressed)
                OnPausePressed?.Invoke();
        }
    }
}