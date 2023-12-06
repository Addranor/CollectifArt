using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BossBattle
{
    public class HealthSystem : MonoBehaviour
    {
        public delegate void OnDeathDelegate();
        public delegate void OnHealthChange(int currentHp, int amount);
        public OnDeathDelegate OnDeath;
        public OnHealthChange OnDamage;
        public OnHealthChange OnHeal;
        
        [Header("Parameters")]
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private float _invulnerabilityTimer = 2f;

        [Header("References")]
        [SerializeField] private Animator _animator;
        
        [Header("Actions")]
        [Space()] public UnityEvent _onDeath;
        [Space()] public UnityEvent _onRevive;

        private float _invulnerabilityCache;
        private int _currentHealth;

        private bool _isInvuln;
        private bool _isDead;

        private static readonly int _IsDead = Animator.StringToHash("isDead");
        private static readonly int _IsBlinking = Animator.StringToHash("isBlinking");

        public bool IsEntityDead() => _isDead;
        public int GetCurHp() => _currentHealth;
        public int GetMaxHp() => _maxHealth;

        public int GetPerHp(int percentage) => percentage * _maxHealth / 100;

        private void Start()
        {
            FillHealth();
        }

        private void Update()
        {
            // Timer
            if (_isInvuln) _invulnerabilityCache -= Time.deltaTime;
            if (_invulnerabilityCache < 0 && _isInvuln) ToggleInvulnerability(false);
        }

        public void FillHealth(int amount = 0)
        {
            _currentHealth = amount == 0 ? _maxHealth : amount;
            if (_isDead) ReviveEntity();
            
            OnHeal?.Invoke(_currentHealth, amount);
        }

        public void TakeDamage(int amount)
        {
            if (_isInvuln || _isDead) return;
            ToggleInvulnerability(true);

            _currentHealth -= amount;
            if (_currentHealth <= 0) KillEntity();

            _animator.SetBool(_IsBlinking, !_isDead);
            OnDamage?.Invoke(_currentHealth, amount);
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
            
            _onDeath.Invoke();
            OnDeath?.Invoke();
        }

        private void ReviveEntity()
        {
            _animator.SetBool(_IsDead, false);
            _isDead = false;
            
            _onRevive.Invoke();
        }

        
    }
}