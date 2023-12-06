using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossBattle
{
    public class BossAI : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Animator _animator;
        [SerializeField] private float _attackTimer = 2f;

        [Header("Attacks")]
        [SerializeField] private int _phase2Trigger;
        [SerializeField] private int _phase3Trigger;

        [Space()]
        [SerializeField] private BossAttack[] _phase1Attacks;
        [SerializeField] private BossAttack[] _phase2Attacks;
        [SerializeField] private BossAttack[] _phase3Attacks;

        private HealthSystem _healthSystem;
        private BattlePhase _currentPhase = BattlePhase.INTRO;
        private float _attackTimerCache;
        private bool _canAttack;
        [SerializeField] private bool _isActive;
        
        private static readonly int _IsBlinking = Animator.StringToHash("isBlinking");

        public void SetAIActive(bool status) => _isActive = status;

        public int GetCurPhasePercentage()
        {
            return _currentPhase switch
            {
                BattlePhase.PHASE_2 => _phase2Trigger,
                BattlePhase.PHASE_3 => _phase3Trigger,
                _ => 100
            };
        }

        private void OnEnable()
        {
            TryGetComponent(out _healthSystem);
            _healthSystem.OnDamage += TakeDamage;
        }

        private void OnDisable()
        {
            _healthSystem.OnDamage -= TakeDamage;
        }

        private void Update()
        {
            if (_isActive)
                Attack();
        }

        private void Attack()
        {
            // Attack Timer
            _attackTimerCache -= Time.deltaTime;
            
            if (_attackTimer < 0) _canAttack = true;
            if (_attackTimerCache > 0 && !_canAttack) return;
            
            _attackTimerCache = _attackTimer;
            _canAttack = false;
            
            switch (_currentPhase)
            {
                case BattlePhase.PHASE_1:
                    // TODO: Trigger Attack
                    Debug.Log("Phase 1 Attack");
                    break;

                case BattlePhase.PHASE_2:
                    // TODO: Trigger Attack
                    Debug.Log("Phase 2 Attack");
                    break;

                case BattlePhase.PHASE_3:
                    // TODO: Trigger Attack
                    Debug.Log("Phase 3 Attack");
                    break;
            }
        }

        private void TakeDamage(int currentHp, int amount)
        {
            if (_healthSystem.GetCurHp() <= 0)
                ChangePhase(BattlePhase.DEAD);
            else if (_healthSystem.GetCurHp() <= _phase3Trigger)
                ChangePhase(BattlePhase.PHASE_3);
            else if (_healthSystem.GetCurHp() <= _phase2Trigger)
                ChangePhase(BattlePhase.PHASE_2);
            else if (_healthSystem.GetCurHp() > _phase2Trigger)
                ChangePhase(BattlePhase.PHASE_1);

            _animator.SetBool(_IsBlinking, _currentPhase != BattlePhase.DEAD);
        }

        private void ChangePhase(BattlePhase battlePhase)
        {
            _currentPhase = battlePhase;
            // Debug.Log("PHASE : " + _currentPhase);
        }

        private enum BattlePhase
        {
            INTRO,
            PHASE_1,
            PHASE_2,
            PHASE_3,
            DEAD
        }
    }
}