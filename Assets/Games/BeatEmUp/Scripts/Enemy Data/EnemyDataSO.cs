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
        
        [Header("Misc Parameters")]
        [SerializeField] private int _damageDealt;
        [SerializeField] private string _animatorPath;

        public string GetName() => _name;
        public float GetSpeed() => _speed;
        public float GetPathLatency() => _pathLatency;
        public Vector2 GetErrorDistributionX() => _errorDistributionX;
        public Vector2 GetErrorDistributionY() => _errorDistributiony;
        
        public int GetMaxHealth() => _maxHealth;
        public float GetInvulnerabilityTimer() => _invulnerabilityTimer;
        
        public int GetDamageDealt() => _damageDealt;
        public string GetAnimatorPath() => _animatorPath;
    }
}