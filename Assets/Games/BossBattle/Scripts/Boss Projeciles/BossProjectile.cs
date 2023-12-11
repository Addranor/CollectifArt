using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BossBattle;
using UnityEngine;

namespace BossBattle
{
    [RequireComponent(typeof(DamageSystem))]
    public class BossProjectile : MonoBehaviour
    {
        public delegate void ProjectileListener(BossProjectile bossProjectile);
        public static ProjectileListener OnProjectileDeath;

        [SerializeField] private bool _dieOnHit;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _timer = 5f;
        [SerializeField] private MovingPattern _movingPattern;
        
        private DamageSystem _damageSystem;
        private Animator _animator;
        private HealthSystem _health;
        private Rigidbody2D _rb;
        
        private PlayerController _player;
        private HealthSystem _playerHealth;
        private Vector2 _playerLastPos;
        private Vector2 _previousPos;
        private float _rbVelocity;
        private float _exponentialSpeed;
        private float _timerCache;
        private bool _isInitialized;
        private bool _isGoingLeft;
        private bool _isDead;
        private BossAttack boss;
        
        private static readonly int IsDead = Animator.StringToHash("isDead");

        private enum MovingPattern
        {
            TO_PLAYER_LAST_POS,
            TO_PLAYER,
            LINEAR,
            LINEAR_EXPONENTIAL
        }

        private void Start() => Initialize(FindAnyObjectByType<PlayerController>());

        public void DestroyProjectile()
        {
            if (boss != null)
                boss.Attack();
            
            Destroy(gameObject);
            OnProjectileDeath?.Invoke(this);
        }
        
        public void DisableCollider()
        {
            _damageSystem.SetDamageDealt(0);
            OnProjectileDeath?.Invoke(this);
        }

        public void Initialize(PlayerController player, bool isGoingLeft = true, BossAttack boss = null)
        {
            TryGetComponent(out _damageSystem);
            TryGetComponent(out _animator);
            TryGetComponent(out _health);
            TryGetComponent(out _rb);
            
            _player = player;
            _player.TryGetComponent(out _playerHealth);

            if (boss != null)
                this.boss = boss;

            if (_health != null)
                _health.OnDeath += OnDeath;

            transform.localScale = new Vector3((_isGoingLeft ? -1 : 1), 1, 1);

            _playerLastPos = player.GetCenter();
            _timerCache = _timer;
            _isGoingLeft = isGoingLeft;
            _isInitialized = true;
        }

        private void OnDisable()
        {
            if (_health != null)
                _health.OnDeath -= OnDeath;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            _timerCache -= Time.deltaTime;
            if (_timerCache < 0) OnDeathCallback();
        }

        private void FixedUpdate()
        {
            if (!_isInitialized) return;
            
            _rbVelocity = ((_rb.position - _previousPos).magnitude) / Time.deltaTime;
            _previousPos = _rb.position;
            
            switch (_movingPattern)
            {
                case MovingPattern.TO_PLAYER:
                    transform.localScale = new Vector3(_player.transform.position.x < 0 ? 1 : -1, 1, 1);
                    _rb.MovePosition(Vector2.MoveTowards(_rb.position, _player.GetCenter(), _speed / 10));
                    break;

                case MovingPattern.LINEAR:
                    transform.localScale = new Vector3(_isGoingLeft ? 1 : -1, 1, 1);
                    _rb.MovePosition(Vector2.MoveTowards(_rb.position, new Vector2(_rb.position.x + (_isGoingLeft ? -1 : 1), _rb.position.y), _speed / 10));
                    break;
                
                case MovingPattern.TO_PLAYER_LAST_POS:
                    transform.localScale = new Vector3(1, 1, 1);
                    _rb.MovePosition(Vector2.MoveTowards(_rb.position, _playerLastPos, _speed / 10));
                    if (_rbVelocity <= 0) OnDeathCallback();
                    break;
                
                case MovingPattern.LINEAR_EXPONENTIAL:
                    if (_exponentialSpeed < _speed)
                        _exponentialSpeed += Time.fixedDeltaTime;

                    else if (_exponentialSpeed > _speed)
                        _exponentialSpeed = _speed;
                    
                    _rb.MovePosition(Vector2.MoveTowards(_rb.position, new Vector2(_rb.position.x + (_isGoingLeft ? -1 : 1), _rb.position.y), _exponentialSpeed / 10));
                    break;
                
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _playerHealth.TakeDamage(_damageSystem.GetDamageDealt());
            if (_dieOnHit)
            {
                if (_health != null)
                    _health.TakeDamage(_health.GetMaxHp());
                else
                    OnDeathCallback();
            }
        }

        private void OnDeath() => OnDeathCallback();

        private void OnDeathCallback()
        {
            _animator.SetTrigger(IsDead);
            //OnProjectileDeath?.Invoke(this);
        }
    }
}