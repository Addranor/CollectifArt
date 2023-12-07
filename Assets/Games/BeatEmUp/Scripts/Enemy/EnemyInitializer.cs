using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeatEmUp
{
    public class EnemyInitializer : MonoBehaviour
    {
        [SerializeField] private bool _spawnOnItsOwn;
        [SerializeField] private EnemyDataSO _defaultData;
        
        [Space()]
        [SerializeField] private Animator _animator;

        private EnemyAI _enemyAI;
        private HealthSystem _health;
        private DamageSystem _damage;

        private void Start()
        {
            if (_spawnOnItsOwn)
                Initialize(_defaultData);
        }

        public void Initialize(EnemyDataSO enemyData)
        {
            _animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("RaccoonControllers/" + enemyData.GetAnimatorPath(), typeof(RuntimeAnimatorController ));

            TryGetComponent(out _enemyAI);
            TryGetComponent(out _health);
            TryGetComponent(out _damage);
            
            _damage.Initialize(enemyData);
            _health.Initialize(enemyData);
            _enemyAI.Initialize(enemyData);
            
            _enemyAI.SetAiActive(true);
        }
    }
}