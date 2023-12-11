using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BossBattle
{
    public class BossTriggers : MonoBehaviour
    {
        [SerializeField] private BossAI _bossAI;
        [SerializeField] private BossAttack _bossAttackSystem;
        private CinemachineImpulseSource _impulseSource;

        public void IntroFinished() => _bossAI.IntroFinished();
        public void AttackFinished() => _bossAI.LastAttackFinished();

        public void Anim_Attack()
        {
            _bossAttackSystem.Attack();   
        }
        
        public void Anim_AttackWithImpulse()
        {
            _bossAttackSystem.Attack();
            _impulseSource.GenerateImpulse();   
        }
        
        public void AttackFinishedWithImpulse()
        {
            _bossAI.LastAttackFinished();
            _impulseSource.GenerateImpulse();
        }

        public void Impulse()
        {
            _impulseSource.GenerateImpulse();
        }

        private void Start() => TryGetComponent(out _impulseSource);
    }
}