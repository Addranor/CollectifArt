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
        [SerializeField] private AudioSource _audioSource;
        private CinemachineImpulseSource _impulseSource;

        public void IntroFinished() => _bossAI.IntroFinished();
        public void AttackFinished() => _bossAI.LastAttackFinished();

        public void Play_Audioclip(AudioClip audioClip) => _audioSource.PlayOneShot(audioClip);
        public void Anim_Attack() => _bossAttackSystem.Attack();
        public void Anim_Attack_With_Audioclip(AudioClip audioClip) { _audioSource.PlayOneShot(audioClip); Anim_Attack(); }
        public void Anim_AttackWithImpulse(AudioClip audioClip)
        {
            if (audioClip != null) _audioSource.PlayOneShot(audioClip);
            _bossAttackSystem.Attack();
            _impulseSource.GenerateImpulse();   
        }
        public void AttackFinishedWithImpulse()
        {
            _bossAI.LastAttackFinished();
            _impulseSource.GenerateImpulse();
        }
        public void Impulse(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
            _impulseSource.GenerateImpulse();
        }

        public void Win() => GameManager.instance.BossDead();
        private void Start() => TryGetComponent(out _impulseSource);
    }
}