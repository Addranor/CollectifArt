using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossBattle
{
    public class BossAI : MonoBehaviour
    {
        public delegate void BossTrigger();
        public static BossTrigger OnIntroFinished;
        
        [Header("Parameters")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _attackTimer = 2f;

        [Header("Attacks")]
        [SerializeField] private int _phase2Trigger;
        [SerializeField] private int _phase3Trigger;

        [Header("Reference")]
        [SerializeField] private HealthSystem _healthSystem;
        
        [Header("SFX")]
        [SerializeField] private AudioClip _phase2Sfx;
        [SerializeField] private AudioClip _phase3Sfx;
        [SerializeField] private AudioClip _damageSfx;
        [SerializeField] private AudioClip _deathSfx;

        private AudioSource _audioSource;
        private BattlePhase _currentPhase = BattlePhase.INTRO;
        private BossAttack _bossAttack;
        private float _attackTimerCache;
        private bool _isAttacking;
        private bool _isActive;
        
        private static readonly int _IsBlinking = Animator.StringToHash("isBlinking");
        private static readonly int Phase2 = Animator.StringToHash("Phase_2");
        private static readonly int Phase3 = Animator.StringToHash("Phase_3");
        private static readonly int IsDead = Animator.StringToHash("isDead");

        public void SetAIActive(bool status) => _isActive = status;
        public void LastAttackFinished() => _isAttacking = false;
        public HealthSystem GetHealthSystem() => _healthSystem;

        private void ChangePhase(BattlePhase battlePhase)
        {
            _currentPhase = battlePhase;

            if (_currentPhase == BattlePhase.PHASE_2) _animator.SetTrigger(Phase2);
            if (_currentPhase == BattlePhase.PHASE_3) _animator.SetTrigger(Phase3);
            if (_currentPhase == BattlePhase.DEAD) _animator.SetTrigger(IsDead);
        }

        public void IntroFinished()
        {
            ChangePhase(BattlePhase.PHASE_1);
            OnIntroFinished?.Invoke();
        }
        
        public int GetCurPhasePercentage()
        {
            switch (_currentPhase)
            {
                case BattlePhase.PHASE_2:
                    return _phase2Trigger;
                
                case BattlePhase.PHASE_3:
                    return _phase3Trigger;
                
                default:
                    return 100;
            }
        }

        private void OnDisable()
        {
            _healthSystem.OnDamage -= TakeDamage;
        }

        private void Start()
        {
            TryGetComponent(out _audioSource);
            TryGetComponent(out _bossAttack);
            
            _healthSystem.InitializeSfx(_damageSfx, _deathSfx);
            _healthSystem.OnDamage += TakeDamage;
            
            _attackTimerCache = _attackTimer;
        }

        private void Update()
        {
            if (_currentPhase == BattlePhase.INTRO) return;
            if (_isActive)
                Attack();
        }

        private void Attack()
        {
            // Check if last attack was finished first
            if (_isAttacking) return;
            
            // Attack Timer
            _attackTimerCache -= Time.deltaTime;
            if (_attackTimerCache > 0) return;
            
            _attackTimerCache = _attackTimer;
            _isAttacking = true;
            
            _bossAttack.GetRandomAttack(_currentPhase);
        }

        private void TakeDamage(int currentHp, int amount)
        {
            if (_healthSystem.GetCurHp() <= 0)
                ChangePhase(BattlePhase.DEAD);
            else if (_healthSystem.GetCurHp() <= _phase3Trigger)
            {
                _audioSource.PlayOneShot(_phase3Sfx);
                ChangePhase(BattlePhase.PHASE_3);
            }
            else if (_healthSystem.GetCurHp() <= _phase2Trigger)
            {
                _audioSource.PlayOneShot(_phase2Sfx);
                ChangePhase(BattlePhase.PHASE_2);
            }
            else if (_healthSystem.GetCurHp() > _phase2Trigger)
                ChangePhase(BattlePhase.PHASE_1);

            _animator.SetBool(_IsBlinking, _currentPhase != BattlePhase.DEAD);
        }
    }
    
    public enum BattlePhase
    {
        INTRO,
        PHASE_1,
        PHASE_2,
        PHASE_3,
        DEAD
    }
}