using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BeatEmUp
{
    public class EnemySpawner : MonoBehaviour
    {
        public delegate void SpawnerDelete(PolygonCollider2D confiner = null);

        public static SpawnerDelete OnSpawnerStart;
        public static SpawnerDelete OnSpawnerClean;
        
        [SerializeField] private PolygonCollider2D _confiner;
        [SerializeField] private EnemyPack _enemyPack;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private GameObject _colliders;
        [SerializeField][Space()] private Transform[] _spawnsPoints;
        [SerializeField][Space()] public UnityEvent _onClear;

        private List<HealthSystem> _enemies;
        private bool _isSpent;
        private int _enemyCount;
        private int _enemyKilled;

        private void Start()
        {
            _enemies = new List<HealthSystem>();
            _enemyCount = _enemyPack.GetEnemyAmount();
            HealthSystem.OnDeath += OnEnemyKilled;
        }

        private void OnDisable()
        {
            HealthSystem.OnDeath -= OnEnemyKilled;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player") && !_isSpent && _enemyPack != null)
                SpawnEnemies();
        }

        private void OnEnemyKilled(HealthSystem entity)
        {
            if (_enemies.Contains(entity)) _enemyKilled++;
            if (_enemyKilled >= _enemyCount) SpawnerClean();
        }

        private void SpawnEnemies()
        {
            
            Vector2 spawnPoint = transform.position;
            int spawnPointsIndex = 0;
            
            foreach (EnemyDataSO enemy in _enemyPack.GetEnemyPack())
            {
                if (_spawnsPoints.Length > 0)
                {
                    if (spawnPointsIndex >= _spawnsPoints.Length) spawnPointsIndex = 0;
                    spawnPoint = _spawnsPoints[spawnPointsIndex].position;
                    spawnPointsIndex++;
                }

                var spawnedEnemy = Instantiate(_enemyPrefab, spawnPoint, Quaternion.identity);
                spawnedEnemy.GetComponent<EnemyInitializer>().Initialize(enemy);
                _enemies.Add(spawnedEnemy.GetComponent<HealthSystem>());
            }
            
            _isSpent = true;
            _colliders.SetActive(true);
            OnSpawnerStart?.Invoke(_confiner);
        }

        private void SpawnerClean()
        {
            _colliders.SetActive(false);
            _onClear?.Invoke();
            OnSpawnerClean?.Invoke(_confiner);
        }
    }
}