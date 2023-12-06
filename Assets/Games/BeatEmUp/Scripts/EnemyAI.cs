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
        private EnemyAI[] _enemies;

        private Rigidbody2D _rb;

        private Vector2 _updatePosition = Vector3.zero;
        private Vector2 _targetPosition = Vector3.zero;
        private Vector2 _velocity;
        private Vector2 _previousPosition;
        
        private bool _isPlayerStopTriggered = false;

        private float _lastPathTime;
        private float _finalErrorDistributionX;
        private float _finalErrorDistributionY;
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private void Start()
        {
            _player = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Exclude);
            _enemies = FindObjectsByType<EnemyAI>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            TryGetComponent(out _rb);

            _updatePosition = transform.position;
        }

        private void Update()
        {
            if (IsHit()) return;
            
            Chase();
            LookAtPlayer();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_updatePosition);
        }

        private bool IsHit()
        {
            return false;
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

        private void LookAtPlayer()
        {
            Vector3 scale = transform.localScale;

            if (_player.transform.position.x > transform.position.x)
                scale.x = Mathf.Abs(scale.x) * -1;

            else
                scale.x = Mathf.Abs(scale.x);
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