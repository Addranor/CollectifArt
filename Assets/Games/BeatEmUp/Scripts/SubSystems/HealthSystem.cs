using System;
using UnityEngine;
using UnityEngine.Events;

namespace BeatEmUp
{
    public class HealthSystem : MonoBehaviour
    {
        public delegate void OnDeathDelegate(HealthSystem entity);
        public delegate void OnHealthChange(int amount);
        public static OnDeathDelegate OnDeath;
        public OnHealthChange OnDamage;
        public OnHealthChange OnHeal;
        
        [Header("Parameters")]
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private float _invulnerabilityTimer = 2f;

        [Header("References")]
        [SerializeField] private Animator _animator;
        
        [Header("Actions")]
        [Space()] public UnityEvent _onDeath;
        [Space()] public UnityEvent _onRevive;

        private AudioSource _audioSource;
        private AudioClip _damageSound;
        private AudioClip _deathSound;
        private float _invulnerabilityCache;
        private bool _isInvuln;
        private bool _isDead;

        private static readonly int _IsDead = Animator.StringToHash("isDead");
        private static readonly int _IsBlinking = Animator.StringToHash("isBlinking");

        public bool IsEntityDead() => _isDead;
        public int GetCurHp() => _currentHealth;
        public int GetMaxHp() => _maxHealth;
        public bool IsInvuln() => _isInvuln;
        public void InitializeSound(AudioClip damagesSfx, AudioClip deathSfx) {  }

        private void Start() => TryGetComponent(out _audioSource);

        public void Initialize(AudioClip damageSfx, AudioClip deathSfx, EnemyDataSO enemyData = null)
        {
            if (enemyData != null)
            {
                _maxHealth = enemyData.GetMaxHealth();
                _invulnerabilityTimer = enemyData.GetInvulnerabilityTimer();
            }
            
            _damageSound = damageSfx;
            _deathSound = deathSfx;
            
            Heal();
        }

        private void Update()
        {
            // Timer
            if (_isInvuln) _invulnerabilityCache -= Time.deltaTime;
            if (_invulnerabilityCache < 0 && _isInvuln) ToggleInvulnerability(false);
        }

        public void Heal(int amount = 0)
        {
            _currentHealth = amount == 0 ? _maxHealth : amount;
            if (_isDead) ReviveEntity();
            
            OnHeal?.Invoke(amount);
        }

        public void TakeDamage(int amount)
        {
            if (_isInvuln || _isDead) return;
            ToggleInvulnerability(true);

            _currentHealth -= amount;
            if (_currentHealth <= 0) KillEntity();
            else _audioSource.PlayOneShot(_damageSound);
            
            _animator.SetBool(_IsBlinking, !_isDead);
            OnDamage?.Invoke(amount);
        }

        private void ToggleInvulnerability(bool state)
        {
            if (state) _invulnerabilityCache = _invulnerabilityTimer;
            
            _animator.SetBool(_IsBlinking, state);
            _isInvuln = state;
        }

        private void KillEntity()
        {
            _isDead = true;
            _animator.SetBool(_IsDead, true);
            
            _audioSource.PlayOneShot(_deathSound);
            _onDeath?.Invoke();
            OnDeath?.Invoke(this);
        }

        private void ReviveEntity()
        {
            _animator.SetBool(_IsDead, false);
            _isDead = false;
            
            _onRevive?.Invoke();
        }

        
    }
}