using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    [CreateAssetMenu(menuName = "Raccoon Enemy")]
    public class EnemyDataSO : ScriptableObject
    {
        [Header("AI Parameters")]
        [SerializeField] private string _name;
        [SerializeField] private float _speed = 0.05f;
        [SerializeField] private float _pathLatency = 1;
        [SerializeField] private Vector2 _errorDistributionX;
        [SerializeField] private Vector2 _errorDistributiony;

        [Header("Health Parameters")]
        [SerializeField] private int _maxHealth;
        [SerializeField] private float _invulnerabilityTimer;

        [Header("Health Parameters")]
        [SerializeField] private WeaponType _weaponType;
        
        [Header("Misc Parameters")]
        [SerializeField] private int _damageDealt;
        [SerializeField] private string _animatorPath;
        
        [Header("Misc Parameters")]
        [SerializeField] private AudioClip _hitSfx;
        [SerializeField] private AudioClip _hitLandedSfx;
        [SerializeField] private AudioClip _damageSfx;
        [SerializeField] private AudioClip _deathSfx;

        public string GetName() => _name;
        public float GetSpeed() => _speed;
        public float GetPathLatency() => _pathLatency;
        public Vector2 GetErrorDistributionX() => _errorDistributionX;
        public Vector2 GetErrorDistributionY() => _errorDistributiony;
        
        public WeaponType GetWeaponType() => _weaponType;
        
        public int GetMaxHealth() => _maxHealth;
        public float GetInvulnerabilityTimer() => _invulnerabilityTimer;
        
        public int GetDamageDealt() => _damageDealt;
        public string GetAnimatorPath() => _animatorPath;

        public AudioClip GetHitSFX() => _hitSfx;
        public AudioClip GetHitLandedSFX() => _hitLandedSfx;
        public AudioClip GetDamageSFX() => _damageSfx;
        public AudioClip GetDeathSFX() => _deathSfx;
    }

    public enum WeaponType
    {
        NONE,
        STICK,
        PIPE
    }
}