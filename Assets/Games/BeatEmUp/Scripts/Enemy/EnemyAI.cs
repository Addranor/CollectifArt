using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeatEmUp
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _speed = 0.05f;
        [SerializeField] private float _pathLatency = 1.0f;
        [SerializeField] private Vector2 _errorDistributionX = Vector2.zero;
        [SerializeField] private Vector2 _errorDistributionY = Vector2.zero;
        
        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _stickSprite;
        [SerializeField] private GameObject _pipeSprite;

        private AudioClip _hitSfx;
        private AudioClip _hitLandedSfx;
        private AudioSource _audioSource;
        private PlayerController _player;
        private HealthSystem _health;
        private DamageSystem _damages;
        private Rigidbody2D _rb;

        private Vector2 _updatePosition = Vector3.zero;
        private Vector2 _targetPosition = Vector3.zero;
        private Vector2 _previousPos;
        
        private bool _isPlayerStopTriggered;
        private bool _canAiMove;
        private bool _isPlayerInRange;
        private bool _hasStick;

        private const float _attackDelayMin = 0.05f;
        private const float _attackDelayMax = 0.20f;
        private const float _attackCooldownMin = 1f;
        private const float _attackCooldownMax = 3f;
        private float _attackDelayCache;
        private float _attackCooldownCache;
        private float _lastPathTime;
        private float _finalErrorDistributionX;
        private float _finalErrorDistributionY;
        private float _rbVelocity;
        
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int IsDamaged = Animator.StringToHash("isDamaged");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsAttackingStick = Animator.StringToHash("isAttackingStick");

        public void SetAiActive(bool canMove) => _canAiMove = canMove;
        public void IsEnemyInRange(bool status) => _isPlayerInRange = status;

        private void OnDisable()
        { 
            _health.OnDamage -= OnDamage;
        }
        
        public void Initialize(EnemyDataSO enemyData)
        {
            _speed = enemyData.GetSpeed();
            _pathLatency = enemyData.GetPathLatency();
            _errorDistributionX = enemyData.GetErrorDistributionX();
            _errorDistributionY = enemyData.GetErrorDistributionY();

            _hitSfx = enemyData.GetHitSFX();
            _hitLandedSfx = enemyData.GetHitLandedSFX();
            
            _player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);

            switch (enemyData.GetWeaponType())
            {
                case WeaponType.STICK:
                    _stickSprite.SetActive(true);
                    _hasStick = true;
                    break;
                
                case WeaponType.PIPE:
                    _pipeSprite.SetActive(true);
                    _hasStick = true;
                    break;
            }

            TryGetComponent(out _audioSource);
            TryGetComponent(out _damages);
            TryGetComponent(out _health);
            TryGetComponent(out _rb);

            _health.OnDamage += OnDamage;

            _updatePosition = transform.position;
        }

        private void Update()
        {
            if (!_canAiMove) return;    // This prevents the AI from moving when attacking.

            if (_attackDelayCache >= 0)
                _attackDelayCache -= Time.deltaTime;

            if (_attackCooldownCache >= 0)
                _attackCooldownCache -= Time.deltaTime;
            
            Attack();
            Chase();
            LookAtPlayer();
        }

        private void FixedUpdate()
        {
            if (!_canAiMove) return;    // This prevents the AI from moving when attacking.
            
            _rbVelocity = ((_rb.position - _previousPos).magnitude) / Time.deltaTime;
            
            _previousPos = _rb.position;
            _rb.MovePosition(_updatePosition);
        }

        private void OnDamage(int amount)
        {
            _animator.SetTrigger(IsDamaged);
        }
        
        private void LookAtPlayer()
        {
            Vector3 scale = transform.localScale;

            if (_player.transform.position.x < transform.position.x)
                scale.x = Mathf.Abs(scale.x) * -1;

            else
                scale.x = Mathf.Abs(scale.x);

            transform.localScale = scale;
        }

        private void Attack()
        {
            if (_attackCooldownCache > 0) return;   // Attack Cooldown
            if (!_isPlayerStopTriggered) return;    // Is in Range
            if (_attackDelayCache > 0) return;      // Delay before attacking when in Range

            _attackCooldownCache = Random.Range(_attackCooldownMin, _attackCooldownMax);
            _animator.SetTrigger(_hasStick ? IsAttackingStick : IsAttacking);
            _audioSource.PlayOneShot(_isPlayerInRange ? _hitLandedSfx : _hitSfx);

            if (_isPlayerInRange)
                _player.Attacked(_damages.GetDamageDealt());
        }
        
        private void Chase()
        {
            _lastPathTime += Time.deltaTime;

            if (_player == null) return;
            
            _animator.SetBool(IsRunning, _rbVelocity > 0);

            var position = transform.position;
            var playerPosition = _player.transform.position;

            if (_lastPathTime > _pathLatency)
            {
                _lastPathTime = 0.0f;

                _finalErrorDistributionX = Random.Range(_errorDistributionX.x, _errorDistributionX.y);
                _finalErrorDistributionY = Random.Range(_errorDistributionY.x, _errorDistributionY.y);

                _targetPosition = new Vector2(playerPosition.x + _finalErrorDistributionX, playerPosition.y + _finalErrorDistributionY);
            }

            _updatePosition = new Vector2(
                Mathf.MoveTowards(position.x, _targetPosition.x, _speed),
                Mathf.MoveTowards(position.y, _targetPosition.y, _speed)
            );

            if (!_isPlayerStopTriggered) return;
            
            _updatePosition.x = transform.position.x;
            _updatePosition.y = Mathf.MoveTowards(position.y, _targetPosition.y, _speed);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("PlayerStopDistance")) return;
            
            _attackDelayCache = Random.Range(_attackDelayMin, _attackDelayMax);
            _isPlayerStopTriggered = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("PlayerStopDistance"))
                _isPlayerStopTriggered = false;
        }
    }
}