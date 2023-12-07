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
        
        [Header("References")]
        [SerializeField] private PlayerSpriteSO _spriteSaveSO;
        [SerializeField] private Animator _animator;
        
        private HealthSystem _health;
        private static readonly int HasStick = Animator.StringToHash("hasStick");

        private void Start()
        {
            TryGetComponent(out _health);

            PlayerController.OnPickUp += OnPickup;
            
            _health.OnDamage += OnDamage;
            _health.OnHeal += OnHeal;
            
            _capeSprite.SetActive(_spriteSaveSO.GetCape());
            _weaponSprite.SetActive(_spriteSaveSO.GetWeapon());
            _shieldSprite.SetActive(_spriteSaveSO.GetShield());
            _bananaSprite.SetActive(_spriteSaveSO.GetBanana());
            _damagesSprite.SetActive(false);
            
            _animator.SetBool(HasStick, _spriteSaveSO.GetWeapon());
        }

        private void OnDisable()
        {
            PlayerController.OnPickUp += OnPickup;
            
            _health.OnDamage -= OnDamage;
            _health.OnHeal -= OnHeal;
        }

        private void OnDamage(int current)
        {
            if (current > _health.GetMaxHp() / 2) return;
            _damagesSprite.SetActive(true);
        }

        private void OnHeal(int current)
        {
            if (current < _health.GetMaxHp() / 2) return;
            _damagesSprite.SetActive(false);
        }

        private void OnPickup(PickUps pickUp)
        {
            switch (pickUp)
            {
                case PickUps.Weapon:
                    _weaponSprite.SetActive(true);
                    _animator.SetBool(HasStick, true);
                    break;
                
                case PickUps.Cape:
                    _capeSprite.SetActive(true);
                    break;
                
                case PickUps.Shield:
                    _shieldSprite.SetActive(true);
                    break;
                
                case PickUps.Banana:
                    _bananaSprite.SetActive(true);
                    break;
            }
        }
    }
}