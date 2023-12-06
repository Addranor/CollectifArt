using UnityEngine;
using UnityEngine.InputSystem;

namespace BeatEmUp
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _playerSprites;
        [SerializeField] private float _speed = 5.0f;

        private Rigidbody2D _rb;
        private Animator _animator;

        private Vector2 _playerInput = Vector2.zero;
        private Vector2 _playerVelocity = Vector2.zero;
        private bool _playerAttack;

        private static readonly int IsRun = Animator.StringToHash("isRun");

        public Rigidbody2D GetRb() => _rb;

        private void Start()
        {
            _playerSprites.TryGetComponent(out _animator);
            TryGetComponent(out _rb);
        }

        private void Update()
        {
            _playerVelocity = _playerInput * _speed;

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

        public void OnMovements(InputValue value) => _playerInput = value.Get<Vector2>();
        public void OnAttack(InputValue value) => _playerAttack = value.isPressed;
    }
}