using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeatEmUp
{
    public class EnemySpawner : MonoBehaviour
    {
        public delegate void SpawnerDelete(PolygonCollider2D confiner = null);

        public static SpawnerDelete OnSpawnerStart;
        public static SpawnerDelete OnSpawnerClean;
        
        [SerializeField] private PolygonCollider2D _confiner;
        [SerializeField] private EnemyPack _enemyPack;
        [SerializeField][Space()] private Transform[] _spawnsPoints;
        
        private bool _isSpent;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player") && !_isSpent)
                SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            
            Vector2 spawnPoint = transform.position;
            int spawnPointsIndex = 0;
            
            foreach (GameObject enemy in _enemyPack.GetEnemyPack())
            {
                if (_spawnsPoints.Length > 0)
                {
                    if (spawnPointsIndex >= _spawnsPoints.Length) spawnPointsIndex = 0;
                    spawnPoint = _spawnsPoints[spawnPointsIndex].position;
                    spawnPointsIndex++;
                }
                
                GameObject spawnedEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity);
            }
            
            _isSpent = true;
            OnSpawnerStart?.Invoke(_confiner);
        }

        private void SpawnerClean()
        {
            OnSpawnerClean?.Invoke(_confiner);
        }
    }
}