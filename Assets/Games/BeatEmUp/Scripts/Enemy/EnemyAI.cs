using System;
using System.Text;
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

        private PlayerController _player;
        private HealthSystem _health;
        private Rigidbody2D _rb;

        private Vector2 _updatePosition = Vector3.zero;
        private Vector2 _targetPosition = Vector3.zero;
        private Vector2 _velocity;
        private Vector2 _previousPosition;
        
        private bool _isPlayerStopTriggered;
        private bool _canAiMove;

        private float _lastPathTime;
        private float _finalErrorDistributionX;
        private float _finalErrorDistributionY;
        
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        public void SetAiActive(bool canMove) => _canAiMove = canMove;

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
            
            _player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);

            TryGetComponent(out _health);
            TryGetComponent(out _rb);

            _health.OnDamage += OnDamage;

            _updatePosition = transform.position;
        }

        private void Update()
        {
            if (!_canAiMove) return;    // This prevents the AI from moving when attacking.
            
            Chase();
            LookAtPlayer();
        }

        private void FixedUpdate()
        {
            if (!_canAiMove) return;    // This prevents the AI from moving when attacking.
            _rb.MovePosition(_updatePosition);
        }

        private void OnDamage(int amount)
        {
            // SetAiActive(false);
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
            Debug.Log("Attack ? More or less");
        }

        private void Chase()
        {
            _lastPathTime += Time.deltaTime;

            if (_player == null) return;

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
            if (other.CompareTag("PlayerStopDistance"))
                _isPlayerStopTriggered = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("PlayerStopDistance"))
                _isPlayerStopTriggered = false;
        }
    }
}