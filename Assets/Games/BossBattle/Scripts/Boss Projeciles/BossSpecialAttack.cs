using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace BossBattle
{
    public class BossSpecialAttack : MonoBehaviour
    {
        public delegate void OnTrigger();
        public OnTrigger OnTriggerEnd;

        private enum BossAttackType
        {
            Stomp,
            Rage
        }

        [SerializeField] private BossAttackType _attackType;
        [SerializeField] private GameObject _specialAttackObject;
        [SerializeField] private Transform[] _randomHeights;
        [SerializeField] public AudioSource _audioSource;
        [SerializeField] public CinemachineImpulseSource _impulse;
        [SerializeField] private bool _debug;

        private PlayerController _player;
        private Animator _animator;
        
        private static readonly int Rage_Param = Animator.StringToHash("Rage");
        private static readonly int Stomp_Param = Animator.StringToHash("Stomp");

        public void TriggerEnd() => OnTriggerEnd?.Invoke();

        public void PickRandomHeight() { _specialAttackObject.transform.localPosition = GetRandomHeight(_randomHeights); }
        public void Stomp(AudioClip audioClip)
        {
            _impulse.GenerateImpulse();
            _audioSource.PlayOneShot(audioClip);
        }

        private void Start()
        {
            _player = FindAnyObjectByType<PlayerController>();
            _animator = GetComponent<Animator>();
            if (_debug) Attack();
        }
        
        // Trigger Attack
        public void Attack()
        {
            switch (_attackType)
            {
                case BossAttackType.Rage:
                    _specialAttackObject.transform.localPosition = GetRandomHeight(_randomHeights);
                    _animator.SetTrigger(Rage_Param);
                    break;
                
                case BossAttackType.Stomp:
                    _specialAttackObject.transform.localPosition = new Vector2(_player.transform.position.x, _specialAttackObject.transform.localPosition.y);
                    _animator.SetTrigger(Stomp_Param);
                    break;
            }
        }

        private Vector2 GetRandomHeight(Transform[] positions)
        {
            if (positions == null && positions.Length == 0) return Vector2.zero;
            return new Vector2(_specialAttackObject.transform.localPosition.x, positions[Random.Range(0, positions.Length)].localPosition.y);
        }
    }
}