using UnityEngine;
using Random = UnityEngine.Random;

namespace BossBattle
{
    public class BossObject : MonoBehaviour
    {
        public delegate void OnTrigger();
        public OnTrigger OnTriggerEnd;
        
        private enum BossObjectType
        {
            Double_Hair,
            Single_Hair,
            Piercing
        }

        [SerializeField] private BossObjectType _objectType;
        [SerializeField] private GameObject[] _gameObjects;
        [SerializeField] private DamageSystem _damageSystem;
        [SerializeField] private bool _debug;

        private AudioSource _audioSource;
        private const float _randomOffset = 2.5f;
        private PlayerController _player;
        private Animation _attack;

        private void Start()
        {
            _player = FindAnyObjectByType<PlayerController>();
            
            TryGetComponent(out _attack);
            TryGetComponent(out _audioSource);
            if (_debug) Attack();
        }

        public void PlaySound(AudioClip audioClip) => _audioSource.PlayOneShot(audioClip);

        public void Attack()
        {
            if (_objectType == BossObjectType.Double_Hair)
                MoveObjectRandom();

            else if (_objectType == BossObjectType.Piercing)
                MoveObjectToPlayer();

            _attack.Play();
        }

        public void TriggerEnd() => OnTriggerEnd?.Invoke();
        public void EnableDamages() => _damageSystem.SetDamageDealt(1);
        public void NullifyDamages() => _damageSystem.SetDamageDealt(0);
        public void PlayAudioClip(AudioClip _audioClip) => _audioSource.PlayOneShot(_audioClip);

        private void MoveObjectRandom()
        {
            var position = transform.position;
            _gameObjects[0].transform.position =
                new Vector3(position.x - _randomOffset + Random.Range(-_randomOffset, _randomOffset - _randomOffset / 2), position.y, 0);
            _gameObjects[1].transform.position =
                new Vector3(position.x + _randomOffset + Random.Range(-_randomOffset - _randomOffset / 2, _randomOffset), position.y, 0);
        }

        private void MoveObjectToPlayer()
        {
            _gameObjects[0].transform.position = new Vector2(_player.transform.position.x, transform.position.y);
        }
    }
}