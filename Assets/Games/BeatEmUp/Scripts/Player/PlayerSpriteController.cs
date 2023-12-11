using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    public enum PickUps
    {
        Weapon,
        Cape,
        Shield,
        Banana
    }
    
    public class PlayerSpriteController : MonoBehaviour
    {
        [Header("Sprites")]
        [SerializeField] private GameObject _weaponSprite;
        [SerializeField] private GameObject _capeSprite;
        [SerializeField] private GameObject _shieldSprite;
        [SerializeField] private GameObject _damagesSprite;
        [SerializeField] private GameObject _bananaSprite;
        
        [Header("SFX")]
        [SerializeField] private AudioClip _weaponPickUpSfx;
        [SerializeField] private AudioClip _capePickUpSfx;
        [SerializeField] private AudioClip _shieldPickUpSfx;
        [SerializeField] private AudioClip _bananaPickUpSfx;
        
        [Header("References")]
        [SerializeField] private SaveGameSO _saveGameSO;
        [SerializeField] private Animator _animator;
        
        private HealthSystem _health;
        private AudioSource _audioSource;
        private PlayerController _controller;
        private static readonly int HasStick = Animator.StringToHash("hasStick");

        public bool HasShield() => _saveGameSO.GetShield();

        private void Start()
        {
            TryGetComponent(out _audioSource);
            TryGetComponent(out _controller);
            TryGetComponent(out _health);

            _health.OnDamage += OnDamage;
            _health.OnHeal += OnHeal;
            
            _capeSprite.SetActive(_saveGameSO.GetCape());
            _weaponSprite.SetActive(_saveGameSO.GetWeapon());
            _shieldSprite.SetActive(_saveGameSO.GetShield());
            _bananaSprite.SetActive(_saveGameSO.GetBanana());
            _damagesSprite.SetActive(false);

            _animator.SetBool(HasStick, _saveGameSO.GetWeapon());
        }

        private void OnDisable()
        {
            _health.OnDamage -= OnDamage;
            _health.OnHeal -= OnHeal;
        }

        private void OnDamage(int current)
        {
            if (_health.GetCurHp() < _health.GetMaxHp() / 2)
                _damagesSprite.SetActive(true);
        }

        private void OnHeal(int current)
        {
            if (_health.GetCurHp() > _health.GetMaxHp() / 2)
                _damagesSprite.SetActive(false);
        }

        public void OnPickup(PickUps pickUp)
        {
            switch (pickUp)
            {
                case PickUps.Weapon:
                    _audioSource.PlayOneShot(_weaponPickUpSfx);
                    _weaponSprite.SetActive(true);
                    _animator.SetBool(HasStick, true);
                    break;
                
                case PickUps.Cape:
                    _audioSource.PlayOneShot(_capePickUpSfx);
                    _capeSprite.SetActive(true);
                    break;
                
                case PickUps.Shield:
                    _audioSource.PlayOneShot(_shieldPickUpSfx);
                    _shieldSprite.SetActive(true);
                    _controller.GetShield();
                    break;
                
                case PickUps.Banana:
                    _audioSource.PlayOneShot(_bananaPickUpSfx);
                    _bananaSprite.SetActive(true);
                    break;
            }
        }
        
        public void SavePickups()
        {
            _saveGameSO.SetWeapon(_weaponSprite.gameObject.activeInHierarchy);
            _saveGameSO.SetCape(_capeSprite.gameObject.activeInHierarchy);
            _saveGameSO.SetShield(_shieldSprite.gameObject.activeInHierarchy);
            _saveGameSO.SetBanana(_bananaSprite.gameObject.activeInHierarchy);
        }
    }
}